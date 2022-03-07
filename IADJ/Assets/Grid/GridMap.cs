using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Grid
{
    public class GridMap : MonoBehaviour
    {
        // Public variables
        public int xSize, zSize;
        public bool drawWireGM, drawCenterGM;

        // Private variables
        private bool _initialized;
        private Cell[,] _gridMap;

        private Cell cellClicked;
        private List<Cell> _allowedCells;
        private List<Cell> _notAllowedCells;

        void Start()
        {
            _allowedCells = new List<Cell>();
            _notAllowedCells = new List<Cell>();
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
            _gridMap = new Cell[xSize, zSize];
            float[] sizeOfMap = GetSizeOfPlane();
            float[] sizeOfCell = GetSizeOfCell();

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    float x = i * sizeOfCell[0] - sizeOfMap[0] / 2 + sizeOfCell[0] / 2;
                    float z = j * sizeOfCell[1] - sizeOfMap[1] / 2 + sizeOfCell[1] / 2;
                    Vector3 center = new Vector3(x, 0, z);
                    _gridMap[i, j] = new Cell(sizeOfCell[0], sizeOfCell[1], center);
                }
            }
        }

        private float[] GetSizeOfPlane()
        {
            float x = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size.x;
            float z = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size.z;
            return new[] {x, z};
        }

        private float[] GetSizeOfCell()
        {
            float x = GetSizeOfPlane()[0] / xSize;
            float z = GetSizeOfPlane()[1] / zSize;
            return new[] {x, z};
        }

        private void OnDrawGizmos()
        {
            if (_initialized)
            {
                DrawGridMap();
                DrawCellClicked();
                DrawCellsAllowance();
            }
        }

        private void DrawCellClicked()
        {
            if (cellClicked != null)
            {
                cellClicked.DrawCellColored(Color.blue);
            }
        }

        private void DrawCellsAllowance()
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

        private void DrawGridMap()
        {
            Handles.color = Color.black;
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    if (drawCenterGM)
                        _gridMap[i, j].DrawCenter();

                    if (drawWireGM)
                        _gridMap[i, j].DrawCell();
                }
            }
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
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    if (_gridMap[i, j].CheckIfCellClicked())
                    {
                        cellClicked = _gridMap[i, j];
                    }
                }
            }
        }


        private void AddAllowedCells()
        {
            _allowedCells = new List<Cell>();
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    if (_gridMap[i, j].CheckCollision() && !_notAllowedCells.Contains(_gridMap[i, j]))
                        _notAllowedCells.Add(_gridMap[i,j]);

                    else if (!_gridMap[i, j].CheckCollision() && !_allowedCells.Contains(_gridMap[i, j]))
                        _allowedCells.Add(_gridMap[i, j]);
                }
            }
        }


        // Metodos de DEBUG
        private void GridMapToString()
        {
            Debug.Log("Start of Grid");
            for (int i = 0; i < xSize; i++)
            {
                Debug.Log("ROW " + i);
                for (int j = 0; j < zSize; j++)
                {
                    Debug.Log(_gridMap[i, j].GetCenter());
                }
            }

            Debug.Log("Finish of Map");
        }
    }
}