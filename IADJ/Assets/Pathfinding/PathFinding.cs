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


        public int MOVE_STRAIGHT = 10;
        public int MOVE_DIAGONAL = 14;


        // Private variables
        private List<Node> _path;


        private void Start()
        {
            _path = new List<Node>();
            InitializeNodeMap();
        }

        private void Update()
        {
            Cell startCell = GetComponent<GridMap>().CheckIfCellClicked(Input.GetKeyUp(KeyCode.Alpha1));
            Cell finishCell = GetComponent<GridMap>().CheckIfCellClicked(Input.GetKeyUp(KeyCode.Alpha2));

            if (startCell != null && finishCell != null && startCell != finishCell)
            {
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                ApplyAstar(startNode, finishNode);
            }
        }

        public void ApplyAstar(Node startNode, Node finishNode)
        {
            if (startNode != null && finishNode != null && startNode != finishNode)
            {
                _path = AStar(startNode, finishNode);
            }
        }


        public List<Node> AStar(Node startNode, Node finalNode)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node> {startNode};

            SetCostsToStart();
            startNode.SetGCost(0);
            startNode.SetHCost(CalculateMono(startNode, finalNode));
            startNode.CalculateFCost();

            Node nodoDebug = null;


            int contador = 0;

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestCostFNode(openList);
                nodoDebug = currentNode;

                if (currentNode.GetCell().GetCoorX() == finalNode.GetCell().GetCoorX() &&
                    currentNode.GetCell().GetCoorZ() == finalNode.GetCell().GetCoorZ())
                {
                    finalNode.SetPreviousNode(currentNode);
                    return CalculatePath(finalNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);
                foreach (Node neighbourNode in GetNeighboursList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    int tentativeGCost = currentNode.GetGCost() + CalculateMono(currentNode, neighbourNode);

                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        Debug.Log("ENTRE AL TENTATIVE COOOOOST");
                        neighbourNode.SetPreviousNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(CalculateMono(neighbourNode, finalNode));
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }

                contador += 1;
            }

            Debug.Log("FALLE");
            return null;
        }

        private List<Node> GetNeighboursList(Node nodo)
        {
            List<Cell> neighbours = GetComponent<GridMap>().GetAllNeighbours(nodo.GetCell()).ToList();

            List<Node> result = new List<Node>();
            foreach (var neighbour in neighbours)
            {
                result.Add(RecoverNodeFromCell(neighbour));
            }

            return result;
        }

        private Node RecoverNodeFromCell(Cell cell)
        {
            return _nodeMap[cell.GetCoorX(), cell.GetCoorZ()];
        }

        private List<Node> CalculatePath(Node finalNode)
        {
            List<Node> path = new List<Node>();
            path.Add(finalNode);
            Node currentNode = finalNode;

            while (currentNode.GetPreviousNode() != null)
            {
                Debug.Log("ENTRE TANTAS VECES");
                path.Add(currentNode.GetPreviousNode());
                currentNode = currentNode.GetPreviousNode();
            }

            Debug.Log("SIZE PATH: " + path.Count);

            path.Reverse();
            return path;
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
            SetCostsToStart();
        }

        public void SetCostsToStart()
        {
            GridMap gridMap = GetComponent<GridMap>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _nodeMap[i, j] = new Node(gridMap.GetCellMap()[i, j]);
                    _nodeMap[i, j].SetGCost(Int32.MaxValue);
                    _nodeMap[i, j].SetHCost(Int32.MaxValue);
                    _nodeMap[i, j].CalculateFCost();
                    _nodeMap[i, j].SetPreviousNode(null);
                }
            }
        }

        // Cost functions
        private int Manhattan(Node start, Node finish)
        {
            return Mathf.Abs(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX()) +
                   Mathf.Abs(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ());
        }

        private int CalculateMono(Node start, Node finish)
        {
            int xDistance = Mathf.Abs(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX());
            int zDistance = Mathf.Abs(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ());
            int remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT * remaining;
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
                        c.GetCell().DrawCellColored(col);
                    }

                    if (drawNumberPath)
                    {
                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = Color.black;
                        style.fontSize = sizeOfTextPath;
                        Handles.Label(c.GetCell().GetCenter(), index.ToString(), style);
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
                Debug.Log("PASO " + index + ":" + c.GetCoorX() + " " + c.GetCoorZ());
            }
        }
    }
}