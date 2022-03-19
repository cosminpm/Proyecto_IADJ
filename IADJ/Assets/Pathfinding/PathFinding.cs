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
        public bool drawNumberPath, drawColorPath, drawCosts;
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
                InitializeNodeMap();
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                ApplyAstar(startNode, finishNode);
            }
        }

        public void ApplyAstar(Node startNode, Node finishNode)
        {
            if (startNode != null && finishNode != null && startNode != finishNode)
            {
                _path.Clear();
                _path = AStar(startNode, finishNode);
            }
        }


        public List<Node> AStar(Node startNode, Node finalNode)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node> {startNode};

            InitializeNodeMap();
            startNode.SetGCost(0);
            startNode.SetHCost(CalculateMono(startNode, finalNode));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestCostFNode(openList);

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
            }

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
            Node currentNode = finalNode;

            while (currentNode.GetPreviousNode() != null)
            {
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
                    _nodeMap[i, j] = new Node(gridMap.GetCellMap()[i, j], Int32.MaxValue, Int32.MaxValue, null);
                    _nodeMap[i, j].SetFCost(Int32.MaxValue);
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
                        style.normal.textColor = Color.red;
                        style.fontSize = sizeOfTextPath;

                        Vector3 pos = new Vector3(c.GetCell().GetCenter().x - 0.1f, c.GetCell().GetCenter().y,
                            c.GetCell().GetCenter().z - 0.1f);
                        Handles.Label(pos, index.ToString(), style);
                    }
                }
            }
        }

        private void DrawCosts()
        {
            String cost = "∞";
            if (drawCosts)
            {
                foreach (var node in _nodeMap)
                {
                    if (node.GetFCost() < Int32.MaxValue)
                    {
                        cost = node.GetFCost().ToString();
                    }
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;
                    style.fontSize = sizeOfTextPath;
                    Handles.Label(node.GetCell().GetCenter(), cost, style);
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawPath();
            DrawCosts();
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