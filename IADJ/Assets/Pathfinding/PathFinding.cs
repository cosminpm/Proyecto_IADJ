using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEditor;
using UnityEngine;

namespace Pathfinding
{
    public class PathFinding : MonoBehaviour
    {
        // Public variables
        public Color startColor = Color.cyan;
        public Color finishColor = Color.red;
        public bool drawNumberPath, drawColorPath;
        public int sizeOfTextPath = 10;
        
        private Node[,] _nodeMap;
        private int _xSize, _zSize;
        
        
        // Private variables
        private List<Cell> _path;
        

        private void Start()
        {
            _path = new List<Cell>();
            InitializeNodeMap();
        }

        private void Update()
        {
            Cell startCell = GetComponent<GridMap>().CheckIfCellClicked(Input.GetKeyDown(KeyCode.Alpha1));
            Cell finishCell = GetComponent<GridMap>().CheckIfCellClicked(Input.GetKeyDown(KeyCode.Alpha2));
            ApllyLRTAStar(startCell, finishCell,GetComponent<GridMap>());
        }


        private List<Cell> LRTAStar(Cell start, Cell finish, GridMap gridMap)
        {
            List<Cell> closed = new List<Cell>();
            closed.Add(start);
            
            Vector3 destino = finish.GetCenter();

            Cell actualCell = new Cell(start);
            Vector3 actual = actualCell.GetCenter();
            actualCell.SetCost(Manhattan(actualCell, finish));
            
            int contador = 0;
            
            while (destino != actual)
            {
                // Get all neighbours and set the cost for each, from the actual cell to the finish cell
                Cell[] neighbours = gridMap.GetAllNeighbours(actualCell);
                foreach (var neighbour in neighbours)
                {
                    int cost = Manhattan(neighbour, finish);
                    neighbour.SetCost(cost);
                }

                // Get the next cell
                Cell nextCell = neighbours[0];
                
                int costeMinimo = Int32.MaxValue;
                foreach (var neighbour in neighbours)
                {
                    if (!closed.Contains(neighbour))
                    {
                        if (costeMinimo == Int32.MaxValue)
                        {
                            nextCell = neighbour;
                            costeMinimo = neighbour.GetCost();
                        }
                        else if (neighbour.GetCost() < nextCell.GetCost())
                        {
                            nextCell = neighbour;
                            costeMinimo = neighbour.GetCost();
                        }
                    }
                }
                
                
                
                closed.Add(nextCell);
                actualCell = nextCell;
                actual = actualCell.GetCenter();
                
                Debug.Log("CA:"+actualCell.GetCoorX()+ " " + actualCell.GetCoorZ() + " COSTE:" + costeMinimo);
                
                //DEBUG
                contador += 1;
                if (contador >= 1000)
                {
                    Debug.Log("ME PETE EN EL CONTADOR, DEMASIADOS PASOS");
                    return closed;
                }
            }
            
            PathToSTRING(closed);
            return closed;
        }

        public bool ApllyLRTAStar(Cell startCell, Cell finishCell, GridMap gridMap)
        {


            if (startCell != null && finishCell != null && startCell != finishCell)
            {
                _path = LRTAStar(startCell, finishCell, gridMap);
                return _path != null;
            }

            return false;
        }


        public List<Node> AStar(Node startNode, Node finalNode)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node> {startNode};

            SetCostsToStart();
            startNode.SetGCost(0);
            startNode.SetHCost(Manhattan(startNode.GetCell(),finalNode.GetCell()));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestCostFNode(openList);
                if (currentNode == finalNode)
                {
                    return CalculatePath(finalNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);


            }
        }

        private List<Node> CalculatePath(Node finalNode)
        {
            return null;
        }


        private Node GetLowestCostFNode(List<Node> pathNodeList)
        {
            Node lowestFNodeCost = pathNodeList[0];
            for (var index = 1; index < pathNodeList.Count; index++)
            {
                if (pathNodeList[index].GetFCost() < lowestFNodeCost.GetFCost())
                    lowestFNodeCost = pathNodeList[index];
            }

            return lowestFNodeCost;
        }
        
        public void InitializeNodeMap()
        {
            GridMap gridMap = GetComponent<GridMap>();
            _xSize = gridMap.GetXSize();
            _zSize = gridMap.GetZSize();
            _nodeMap = new Node[_xSize, _zSize];
        }

        public void SetCostsToStart()
        {
            GridMap gridMap = GetComponent<GridMap>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _nodeMap[i, j] = new Node(gridMap.GetCellMap()[i,j]);
                    _nodeMap[i, j].SetGCost(Int32.MaxValue);
                    _nodeMap[i, j].CalculateFCost();
                    _nodeMap[i,j].SetPreviousNode(null);
                }
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
        
        // Draw Methods
        private void DrawPath()
        {
            if (_path != null)
            {
                int pathLen = _path.Count;
                for (var index = 0; index < _path.Count; index++)
                {
                    var c = _path[index];
                    if (drawColorPath)
                    {
                        
                        float t = index / (float) pathLen;
                        Color col = Color.Lerp(startColor, finishColor, t);
                        c.DrawCellColored(col);  
                    }
                    if (drawNumberPath)
                    {
                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = Color.black;
                        style.fontSize = sizeOfTextPath;
                        Handles.Label(c.GetCenter(),index.ToString(),style);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawPath();
        }
        
        // Debug Methods
        private void PathToSTRING(List<Cell> lista)
        {
            for (var index = 0; index < lista.Count; index++)
            {
                var c = lista[index];
                Debug.Log("PASO "+index+ ":" +c.GetCoorX()+ " " + c.GetCoorZ());
            }
        } 


    }
}