using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Grid
{
    public class GridMap : MonoBehaviour
    {
        public const int LIMITER_CELLS = 1000;
        // Public variables
        private int _xSize, _zSize;
        public bool drawWireGm, drawCenterGm, drawAllowedCells;

        // Private variables
        private bool _initialized;
        private Cell[,] _gridMap;

        private Cell _cellClicked;
        private List<Cell> _allowedCells;
        private List<Cell> _notAllowedCells;
        
        private float _sizePlaneX,_sizePlaneZ;

        void Start()
        {
            _allowedCells = new List<Cell>();
            _notAllowedCells = new List<Cell>();
            
            GetSizeXAndSizeZ();
            CreateGrid();
            _initialized = true;
            AddAllowedCells();
            CreateAllBoxColliders();
        }

        void Update()
        {
            CheckIfCellClicked();
        }

        private void CreateGrid()
        {
            _gridMap = new Cell[_xSize, _zSize];

            float[] sizeOfCell = GetSizeOfCell();
            Debug.Log(sizeOfCell[0]);
            
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    //float x = i * sizeOfCell[0] - _sizePlaneX / 2 + sizeOfCell[0] / 2;
                    //float z = j * sizeOfCell[1] - _sizePlaneZ / 2 + sizeOfCell[1] / 2;
                    
                    float x = i * sizeOfCell[0]+ sizeOfCell[0];
                    float z = j * sizeOfCell[1]+ sizeOfCell[1];
                    
                    Vector3 center = new Vector3(x, 0, z);
                    _gridMap[i, j] = new Cell(sizeOfCell[0], sizeOfCell[1], center);
                }
            }
        }


        private float GreatestCommonDivisor(float a, float b)
        {
            float res;
            do
            {
                res = b;
                b = a % b;
                a = res;
            } while (b != 0);

            return res;
        }

        // Method if terrain is only One Plane
        private float[] GetSizeOfPlane()
        {
            String nombreUnPlano = "Plane";
            float x = GameObject.Find(nombreUnPlano).GetComponent<Renderer>().bounds.size.x;
            float z = GameObject.Find(nombreUnPlano).GetComponent<Renderer>().bounds.size.z;
            return new[] {x, z};
        }
        
        // Method if terrain are multiple plains
        private float[] GetSizeOfMultiplePlains()
        {
            float x = 0;
            float z = 0;
            
            String nameParent = "MazeFormations";
            String nameFloor = "Floor(Clone)";

            GameObject terrain = GameObject.Find(nameParent);
            int nChild = terrain.transform.childCount;
            for (int i = 0; i < nChild; i++)
            {
                Transform child = terrain.transform.GetChild(i);
                if (child.name.Equals(nameFloor))
                {
                    x += child.GetComponent<Renderer>().bounds.size.x;
                    z += child.GetComponent<Renderer>().bounds.size.z;
                }
            }
            Debug.Log("AA:"+x);
            Debug.Log("bb:"+z);
            return new[] {x, z};
        }
        
        private float[] GetSizeOfCell()
        {
            float x = _sizePlaneX / _xSize;
            float z = _sizePlaneZ / _zSize;

            return new[] {x, z};
        }
        
        private void CreateAllBoxColliders()
        {
            GameObject parent = new GameObject();
            parent.name = "parent";
            foreach (var cell in _notAllowedCells)
            {
                cell.CreateBoxCollider(parent);
            }
        }

        private void CheckIfCellClicked()
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    if (_gridMap[i, j].CheckIfCellClicked())
                    {
                        _cellClicked = _gridMap[i, j];
                    }
                }
            }
        }
        
        private void AddAllowedCells()
        {
            _allowedCells = new List<Cell>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    if (_gridMap[i, j].CheckCollision() && !_notAllowedCells.Contains(_gridMap[i, j]))
                        _notAllowedCells.Add(_gridMap[i,j]);

                    else if (!_gridMap[i, j].CheckCollision() && !_allowedCells.Contains(_gridMap[i, j]))
                        _allowedCells.Add(_gridMap[i, j]);
                }
            }
        }


        private void GetSizeXAndSizeZ()
        {
            float[] sizes = GetSizeOfMultiplePlains();
            
            _sizePlaneX = sizes[0]/10;
            _sizePlaneZ = sizes[1]/10;
            
            float greatestCommonDivisor = GreatestCommonDivisor(_sizePlaneX, _sizePlaneZ);
            _xSize = (int) (_sizePlaneX / greatestCommonDivisor);
            _zSize = (int) (_sizePlaneZ / greatestCommonDivisor);

            int numCells = _xSize * _zSize;

            while (numCells < LIMITER_CELLS)
            {
                _xSize *= 2;
                _zSize *= 2;
                numCells = _xSize * _zSize;
            }

        }
        
        
        // Draw Methods
        private void OnDrawGizmos()
        {
            if (_initialized)
            {
                DrawGridMap();
                DrawCellClicked();
                DrawCellsAllowance();
            }
            Handles.color = Color.green;
        }

        private void DrawCellClicked()
        {
            if (_cellClicked != null)
            {
                _cellClicked.DrawCellColored(Color.blue);
            }
        }

        private void DrawCellsAllowance()
        {
            if (drawAllowedCells)
            {
                foreach (var cell in _allowedCells)
                {
                    Color c = new Color(.25f, .25f, .95f, 0.75f);
                    cell.DrawCellColored(c);
                }

                foreach (var cell in _notAllowedCells)
                {
                    Color c = new Color(0.952f, 0.286f, 0.286f, 0.75f);
                    cell.DrawCellColored(c);
                }
            }
        }

        private void DrawGridMap()
        {
            Handles.color = Color.black;
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    if (drawCenterGm)
                        _gridMap[i, j].DrawCenter();

                    if (drawWireGm)
                        _gridMap[i, j].DrawCell();
                }
            }
        }

        // Metodos de DEBUG
        private void GridMapToString()
        {
            Debug.Log("Start of Grid");
            for (int i = 0; i < _xSize; i++)
            {
                Debug.Log("ROW " + i);
                for (int j = 0; j < _zSize; j++)
                {
                    Debug.Log(_gridMap[i, j].GetCenter());
                }
            }

            Debug.Log("Finish of Map");
        }
    }
}