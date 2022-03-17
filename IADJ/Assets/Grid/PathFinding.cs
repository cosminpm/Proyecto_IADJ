using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class PathFinding : MonoBehaviour
    {
        private List<Cell> LRTAStar(Cell start, Cell finish, GridMap gridMap)
        {
            //List<Cell> path = new List<Cell>();
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


        private void Update()
        {
            
        }

        private void ApllyLRTAStar(Cell start, Cell finish, GridMap gridMap)
        {
            if (start != null && finish != null)
            {
                List<Cell> path = LRTAStar(start, finish, gridMap);
            }
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