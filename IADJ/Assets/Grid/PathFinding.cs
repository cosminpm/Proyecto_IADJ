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
            Debug.Log("PathLenght:"+_path.Count);
        }


        private List<Cell> LRTAStar(Cell start, Cell finish, GridMap gridMap)
        {
            List<Cell> closed = new List<Cell>();
            Cell actualCell = start;
            while (actualCell != finish)
            {
                // Get all neighbours and set the cost for each, from the actual cell to the finish cell
                Cell[] neighbours = gridMap.GetAllNeighbours(actualCell);
                actualCell.SetCost(0);
                
                foreach (var n in neighbours)
                {
                    int cost = Manhattan(n, finish);
                    n.SetCost(cost);
                }
                
                // Get the next cell
                int maxCost = int.MaxValue;
                var nextCell = neighbours[0];
                
                foreach (var n in neighbours)
                {
                    if (!closed.Contains(n) && n.GetCost() < maxCost)
                    {
                        nextCell = n;
                        maxCost = n.GetCost();
                    }
                }
                closed.Add(nextCell);
                actualCell = nextCell;
            }
            return closed;
        }

        public bool ApllyLRTAStar(GridMap gridMap)
        {
            startCell = gridMap.CheckIfCellClicked(Input.GetKeyDown(KeyCode.S));
            finishCell = gridMap.CheckIfCellClicked(Input.GetKeyDown(KeyCode.F));
            
            if (startCell != null && finishCell != null)
            {
                _path = LRTAStar(startCell, finishCell, gridMap); ;
                return true;
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
                    Color col = Color.Lerp(startColor, finishColor, index / pathLen);
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