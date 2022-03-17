using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Grid
{
    public class GridMap : MonoBehaviour
    {
        public int LIMITER_CELLS = 4000;

        // Public variables
        private int _xSize, _zSize;
        public bool drawWireGm, drawCenterGm, drawAllowedCells, drawCellClicked;
        public float intensityAllowedCells = 0.5f;
        public int modeOfTerrain;

        // Private variables
        private bool _initialized;
        private Cell[,] _gridMap;

        private Cell _cellClicked;
        private List<Cell> _allowedCells;
        private List<Cell> _notAllowedCells;

        private float _sizePlaneX, _sizePlaneZ, _heightTerrain;

        public String nameFloor = "Floor(Clone)";
        public String nameParent = "MazePathFinderLRTA";
        public String tagFloor = "Floor";
        
        private List<Cell> _cellsAdyacent;


        void Start()
        {
            _allowedCells = new List<Cell>();
            _notAllowedCells = new List<Cell>();
            _cellsAdyacent = new List<Cell>();

            GetSizeXAndSizeZ();
            CreateGrid();
            _initialized = true;
            AddAllowedCells();
            CreateAllBoxColliders();
        }

        void Update()
        {
        }

        private void CreateGrid()
        {
            _gridMap = new Cell[_xSize, _zSize];

            float[] sizeOfCell = GetSizeOfCell();

            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    float x, z;
                    Vector3 primerVector3 = GetCornetTopLeft();
                    x = i * sizeOfCell[0] + sizeOfCell[0] / 2;
                    z = j * sizeOfCell[1] + sizeOfCell[1] / 2;
                    x += primerVector3.x;
                    z += primerVector3.z;

                    Vector3 center = new Vector3(x, _heightTerrain, z);
                    _gridMap[i, j] = new Cell(sizeOfCell[0], sizeOfCell[1], center, i, j);
                }
            }
        }

        private Vector3 GetCornetTopLeft()
        {
            GameObject father = GameObject.Find(nameParent);
            if (modeOfTerrain != 0)
                return CornerTopLeftMultiple(father);

            else
                return CornerOnePlane(father);
        }

        private Vector3 CornerTopLeftMultiple(GameObject father)
        {
            Vector3 corner = new Vector3();
            int nChild = father.transform.childCount;
            for (int i = 0; i < nChild; i++)
            {
                Transform child = father.transform.GetChild(i);
                if (child.name.Equals(nameFloor))
                {
                    corner = child.GetComponent<Renderer>().bounds.center -
                             child.GetComponent<Renderer>().bounds.extents;
                    break;
                }
            }

            return corner;
        }

        private Vector3 CornerOnePlane(GameObject father)
        {
            return father.GetComponent<Renderer>().bounds.center - father.GetComponent<Renderer>().bounds.extents;
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
            String nombreUnPlano = nameParent;
            float x = GameObject.Find(nombreUnPlano).GetComponent<Renderer>().bounds.size.x;
            float z = GameObject.Find(nombreUnPlano).GetComponent<Renderer>().bounds.size.z;
            _heightTerrain = GameObject.Find(nombreUnPlano).transform.position.y;
            return new[] {x, z};
        }

        // Method if terrain are multiple plains
        private float[] GetSizeOfMultiplePlains()
        {
            float x = 0;
            float z = 0;

            GameObject terrain = GameObject.Find(nameParent);
            int nChild = terrain.transform.childCount;
            for (int i = 0; i < nChild; i++)
            {
                Transform child = terrain.transform.GetChild(i);
                if (child.name.Equals(nameFloor))
                {
                    x += child.GetComponent<Renderer>().bounds.size.x;
                    z += child.GetComponent<Renderer>().bounds.size.z;
                    _heightTerrain = child.transform.position.y;
                }
            }

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

        public Cell CheckIfCellClicked(bool input)
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    if (_gridMap[i, j].CheckIfCellClicked(input))
                    {
                        _cellClicked = _gridMap[i, j];
                    }
                }
            }
            return _cellClicked;
        }

        private void AddAllowedCells()
        {
            _allowedCells = new List<Cell>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    if (_gridMap[i, j].CheckCollision() && !_notAllowedCells.Contains(_gridMap[i, j]))
                        _notAllowedCells.Add(_gridMap[i, j]);

                    else if (!_gridMap[i, j].CheckCollision() && !_allowedCells.Contains(_gridMap[i, j]))
                        _allowedCells.Add(_gridMap[i, j]);
                }
            }
        }

        private void GetSizeXAndSizeZ()
        {
            float[] sizes;
            if (modeOfTerrain == 0)
            {
                sizes = GetSizeOfPlane();
            }
            else
            {
                sizes = GetSizeOfMultiplePlains();
                sizes[0] /= 10;
                sizes[1] /= 10;
            }


            _sizePlaneX = sizes[0];
            _sizePlaneZ = sizes[1];

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

        // Methods for LRTA*

        public Cell[] GetHorizontalCells(Cell c)
        {
            Cell top, bot, left, right;
            top = bot = left = right = null;
            if ((c.GetCoorX() + 1 < _xSize) && _gridMap[c.GetCoorX() + 1, c.GetCoorZ()].GetIsAllowedCell())
                right = _gridMap[c.GetCoorX() + 1, c.GetCoorZ()];

            if ((c.GetCoorZ() + 1 < _zSize) && _gridMap[c.GetCoorX(), c.GetCoorZ() + 1].GetIsAllowedCell())
                bot = _gridMap[c.GetCoorX(), c.GetCoorZ() + 1];

            if ((c.GetCoorX() - 1 >= 0) && _gridMap[c.GetCoorX() - 1, c.GetCoorZ()].GetIsAllowedCell())
                left = _gridMap[c.GetCoorX() - 1, c.GetCoorZ()];

            if ((c.GetCoorZ() - 1 >= 0) && _gridMap[c.GetCoorX(), c.GetCoorZ() - 1].GetIsAllowedCell())
                top = _gridMap[c.GetCoorX(), c.GetCoorZ() - 1];
            return new[] {right, left, top, bot};
        }

        public Cell[] GetDiagonalCells(Cell c)
        {
            Cell topLeft, topRight, botLeft, botRight;
            topLeft = topRight = botLeft = botRight = null;
            if ((c.GetCoorX() + 1 < _xSize) &&
                (c.GetCoorZ() - 1 >= 0) &&
                _gridMap[c.GetCoorX() + 1, c.GetCoorZ() - 1].GetIsAllowedCell())
                topRight = _gridMap[c.GetCoorX() + 1, c.GetCoorZ() - 1];

            if ((c.GetCoorX() + 1 < _xSize) &&
                (c.GetCoorZ() + 1 < _zSize) &&
                _gridMap[c.GetCoorX() + 1, c.GetCoorZ() + 1].GetIsAllowedCell())
                botRight = _gridMap[c.GetCoorX() + 1, c.GetCoorZ() + 1];

            if ((c.GetCoorX() - 1 >= 0) &&
                (c.GetCoorZ() - 1 >= 0) &&
                _gridMap[c.GetCoorX() - 1, c.GetCoorZ() - 1].GetIsAllowedCell())
                topLeft = _gridMap[c.GetCoorX() - 1, c.GetCoorZ() - 1];

            if ((c.GetCoorZ() + 1 < _zSize) &&
                (c.GetCoorX() - 1 >= 0) &&
                _gridMap[c.GetCoorX() - 1, c.GetCoorZ() + 1].GetIsAllowedCell())
                botLeft = _gridMap[c.GetCoorX() - 1, c.GetCoorZ() + 1];
            return new[] {topLeft, topRight, botLeft, botRight};
        }

        public Cell[] GetAllNeighbours(Cell c)
        {
            List<Cell> c1 = GetDiagonalCells(c).ToList();
            List<Cell> c2 = GetHorizontalCells(c).ToList();
            c1.AddRange(c2);
            c1.RemoveAll(item => item == null);
            return c1.ToArray();
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
        }

        private void DrawAdyacentCells(Cell c)
        {
            List<Cell> cells = GetDiagonalCells(c).ToList();
            List<Cell> cells2 = GetHorizontalCells(c).ToList();
            cells.AddRange(cells2);
        
            foreach (var i in cells)
            {
                if (i != null)
                {
                    i.DrawCellColored(Color.magenta);
                }
            }
        }
        
        private void DrawCellClicked()
        {
            if (drawCellClicked && _cellClicked != null)
            {
                Color c = new Color(1, 1, 1, 1);
                _cellClicked.DrawCellColored(Color.black);
                DrawAdyacentCells(_cellClicked);
            }
        }

        private void DrawCellsAllowance()
        {
            if (drawAllowedCells)
            {
                foreach (var cell in _allowedCells)
                {
                    Color c = new Color(.15f, .15f, .95f, intensityAllowedCells);
                    cell.DrawCellColored(c);
                }

                foreach (var cell in _notAllowedCells)
                {
                    Color c = new Color(0.952f, 0.286f, 0.286f, intensityAllowedCells);
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