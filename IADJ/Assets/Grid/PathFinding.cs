using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class PathFinding : MonoBehaviour
    {
        // Public variables
        public Color startColor = Color.cyan;

        public Color finishColor = Color.red;

        // Private variables
        private List<Cell> _path;
        private Cell startCell;
        private Cell finishCell;

        private void Start()
        {
            _path = new List<Cell>();
        }

        private void Update()
        {
            ApllyLRTAStar(GetComponent<GridMap>());
        }


        private List<Cell> LRTAStar(Cell start, Cell finish, GridMap gridMap)
        {
            List<Cell> closed = new List<Cell>();

            Cell startCellLr = new Cell(start);
            Cell finishCellLr = new Cell(finish);

            Cell actualCell = startCellLr;
            actualCell.SetCost(Manhattan(actualCell, finish));
            closed.Add(start);

            int contador = 0;

            while (!(actualCell.GetCoorX() == finishCellLr.GetCoorX() &&
                     actualCell.GetCoorZ() == finishCellLr.GetCoorZ()))
            {
                // Get all neighbours and set the cost for each, from the actual cell to the finish cell
                Cell[] neighbours = gridMap.GetAllNeighbours(actualCell);
                actualCell.SetCost(0);

                foreach (var n in neighbours)
                {
                    int cost = Manhattan(n, finishCellLr);
                    n.SetCost(cost);
                }

                // Get the next cell
                int minCost = int.MaxValue;

                Cell nextCell = neighbours[0];

                foreach (var n in neighbours)
                {
                    if (!closed.Contains(n))
                    {
                        if (minCost == int.MaxValue)
                        {
                            nextCell = n;
                            minCost = n.GetCost();
                        }
                        else if (n.GetCost() < nextCell.GetCost())
                        {
                            nextCell = n;
                            minCost = n.GetCost();
                        }
                    }
                }

                if (nextCell == null)
                {
                    return null;
                }

                closed.Add(nextCell);
                actualCell = nextCell;

                //DEBUG
                contador += 1;
                if (contador >= 200)
                    return closed;
            }

            return closed;
        }

        public bool ApllyLRTAStar(GridMap gridMap)
        {
            startCell = gridMap.CheckIfCellClicked(Input.GetKeyDown(KeyCode.Alpha1));
            finishCell = gridMap.CheckIfCellClicked(Input.GetKeyDown(KeyCode.Alpha2));

            if (startCell != null && finishCell != null && startCell != finishCell)
            {
                _path = LRTAStar(startCell, finishCell, gridMap);
                return _path != null;
            }

            return false;
        }

        public void DrawPath()
        {
            if (_path != null)
            {
                int pathLen = _path.Count;
                for (var index = 0; index < _path.Count; index++)
                {
                    var c = _path[index];
                    float t = index / (float) pathLen;
                    Debug.Log("I:" + index);
                    Debug.Log("PL:" + pathLen);
                    Debug.Log("T:" + t);
                    Color col = Color.Lerp(startColor, finishColor, t);
                    c.DrawCellColored(col);
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawPath();
        }

        // Cost functions
        private int Manhattan(Cell start, Cell finish)
        {
            return Mathf.Abs(start.GetCoorX() - finish.GetCoorX()) + Mathf.Abs(start.GetCoorZ() - finish.GetCoorZ());
        }

        private int Chebychev(Cell start, Cell finish)
        {
            return 0;
        }
    }
}