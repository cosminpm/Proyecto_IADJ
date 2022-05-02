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
        public bool drawWireGm, drawCenterGm, drawAllowedCells, drawCellClicked, drawCellNumber;
        public float intensityAllowedCells = 0.5f;
        public int sizeOfTextGrid = 10;
        public String nameParent = "";

        public String tagFloor = "";

        // Private variables
        private bool _initialized;
        private Cell[,] _cellMap;

        private Cell _cellClicked;
        private List<Cell> _allowedCells;
        private List<Cell> _notAllowedCells;

        private float _sizePlaneX, _sizePlaneZ, _heightTerrain;


        private List<Cell> _cellsAdyacent;

        public enum TipoTerreno
        {
            Camino = 0,
            Pradera = 1,
            Bosque = 2,
            Acantilado = 3,
            Rio = 4,
            BaseRoja = 5,
            BaseAzul = 6,
        }

        void Awake()
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

        private void CreateGrid()
        {
            _cellMap = new Cell[_xSize, _zSize];
            float[] sizeOfCell = GetSizeOfCell();

            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    float x, z;
                    Vector3 primerVector3 = GetCornetTopLeft(GameObject.Find(nameParent));
                    x = i * sizeOfCell[0] + sizeOfCell[0] / 2;
                    z = (-j + 1) * sizeOfCell[1] + sizeOfCell[1] / 2;
                    x += primerVector3.x;
                    z += primerVector3.z;
                    Vector3 center = new Vector3(x, _heightTerrain, z);
                    _cellMap[i, j] = new Cell(sizeOfCell[0], sizeOfCell[1], center, i, j);
                }
            }
        }

        private Vector3 GetCornetTopLeft(GameObject parent)
        {
            Vector3 corner = parent.transform.GetChild(0).GetComponent<Renderer>().bounds.center -
                             parent.transform.GetChild(0).GetComponent<Renderer>().bounds.extents;
            return corner;
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

        // Method if terrain are multiple plains
        private float[] GetSizeOfMultiplePlains()
        {
            float x,z;
            GameObject terrain = GameObject.Find(nameParent);
            int nChild = terrain.transform.childCount;
            Transform child = terrain.transform.GetChild(nChild - 1);
            var strings = child.name.Split('_');
            x = (float.Parse(strings[2])) * child.transform.GetComponent<Renderer>().bounds.size.x;
            z = (float.Parse(strings[1]) + 1) * child.transform.GetComponent<Renderer>().bounds.size.z;
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
                    if (_cellMap[i, j].CheckIfCellClicked(input))
                    {
                        _cellClicked = _cellMap[i, j];
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
                    if (_cellMap[i, j].CheckCollision() && !_notAllowedCells.Contains(_cellMap[i, j]))
                        _notAllowedCells.Add(_cellMap[i, j]);

                    else if (!_cellMap[i, j].CheckCollision() && !_allowedCells.Contains(_cellMap[i, j]))
                        _allowedCells.Add(_cellMap[i, j]);
                }
            }
        }

        private void GetSizeXAndSizeZ()
        {
            float[] sizes;
            sizes = GetSizeOfMultiplePlains();
            _sizePlaneX = sizes[0];
            _sizePlaneZ = sizes[1];
            float greatestCommonDivisor = GreatestCommonDivisor(_sizePlaneX, _sizePlaneZ);
            _zSize = (int) (_sizePlaneZ / greatestCommonDivisor);
            _xSize = (int) (_sizePlaneX / greatestCommonDivisor);
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
            if ((c.GetCoorX() + 1 < _xSize) && _cellMap[c.GetCoorX() + 1, c.GetCoorZ()].GetIsAllowedCell())
                right = _cellMap[c.GetCoorX() + 1, c.GetCoorZ()];

            if ((c.GetCoorZ() + 1 < _zSize) && _cellMap[c.GetCoorX(), c.GetCoorZ() + 1].GetIsAllowedCell())
                bot = _cellMap[c.GetCoorX(), c.GetCoorZ() + 1];

            if ((c.GetCoorX() - 1 >= 0) && _cellMap[c.GetCoorX() - 1, c.GetCoorZ()].GetIsAllowedCell())
                left = _cellMap[c.GetCoorX() - 1, c.GetCoorZ()];

            if ((c.GetCoorZ() - 1 >= 0) && _cellMap[c.GetCoorX(), c.GetCoorZ() - 1].GetIsAllowedCell())
                top = _cellMap[c.GetCoorX(), c.GetCoorZ() - 1];
            return new[] {right, left, top, bot};
        }

        public Cell[] GetDiagonalCells(Cell c)
        {
            Cell topLeft, topRight, botLeft, botRight;
            topLeft = topRight = botLeft = botRight = null;
            if ((c.GetCoorX() + 1 < _xSize) &&
                (c.GetCoorZ() - 1 >= 0) &&
                _cellMap[c.GetCoorX() + 1, c.GetCoorZ() - 1].GetIsAllowedCell())
                topRight = _cellMap[c.GetCoorX() + 1, c.GetCoorZ() - 1];

            if ((c.GetCoorX() + 1 < _xSize) &&
                (c.GetCoorZ() + 1 < _zSize) &&
                _cellMap[c.GetCoorX() + 1, c.GetCoorZ() + 1].GetIsAllowedCell())
                botRight = _cellMap[c.GetCoorX() + 1, c.GetCoorZ() + 1];

            if ((c.GetCoorX() - 1 >= 0) &&
                (c.GetCoorZ() - 1 >= 0) &&
                _cellMap[c.GetCoorX() - 1, c.GetCoorZ() - 1].GetIsAllowedCell())
                topLeft = _cellMap[c.GetCoorX() - 1, c.GetCoorZ() - 1];

            if ((c.GetCoorZ() + 1 < _zSize) &&
                (c.GetCoorX() - 1 >= 0) &&
                _cellMap[c.GetCoorX() - 1, c.GetCoorZ() + 1].GetIsAllowedCell())
                botLeft = _cellMap[c.GetCoorX() - 1, c.GetCoorZ() + 1];
            return new[] {topLeft, topRight, botLeft, botRight};
        }

        public Cell[] GetAllNeighbours(Cell c)
        {
            return GetAllNeighbours(c, 999);
        }

        public Cell[] GetAllNeighbours(Cell c, int heuristicApply)
        {
            List<Cell> c1 = new List<Cell>();
            if (heuristicApply != 0)
                c1 = GetDiagonalCells(c).ToList();
            List<Cell> c2 = GetHorizontalCells(c).ToList();
            c1.AddRange(c2);
            c1.RemoveAll(item => item == null);
            return c1.ToArray();
        }

        public Cell WorldToMap(Vector3 v)
        {
            foreach (var cell in _cellMap)
            {
                if (cell.CheckIfVector3InsideBox(v))
                {
                    return cell;
                }
            }

            throw new Exception("The player is not in the grid");
        }


        // GET and SET Methods
        public int GetXSize()
        {
            return _xSize;
        }

        public int GetZSize()
        {
            return _zSize;
        }

        public Cell[,] GetCellMap()
        {
            return _cellMap;
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
                _cellClicked.DrawCellColored(Color.black);
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
                        _cellMap[i, j].DrawCenter();

                    if (drawWireGm)
                        _cellMap[i, j].DrawCell();
                    if (drawCellNumber)
                        _cellMap[i, j].DrawCellNumber(sizeOfTextGrid);
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
                    Debug.Log(_cellMap[i, j].GetCenter());
                }
            }

            Debug.Log("Finish of Map");
        }
    }
}