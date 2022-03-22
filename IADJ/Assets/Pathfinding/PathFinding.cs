using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEditor;
using UnityEditorInternal;
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
        public int HEURISTIC;
        private Node[,] _nodeMap;

        private int _xSize, _zSize;


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
            HEURISTIC = 0;

            //ApplyAStar(startCell,finishCell,HEURISTIC);
            ApplyLRTA(startCell, finishCell);
        }

        public int HeuristicApply(Node startNode, Node finishNode, int heuristic)
        {
            switch (heuristic)
            {
                case 0:
                    return Manhattan(startNode, finishNode);
                case 1:
                    return Chebychev(startNode, finishNode);
                case 2:
                    return Euclidean(startNode, finishNode);
                default:
                    return Manhattan(startNode, finishNode);
            }
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
                currentNode = currentNode.GetPreviousNode();
                path.Add(currentNode);
            }

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

        // Functions of Finding Path



        private void SetHeuristicOfEveryOne(Node finishNode)
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _nodeMap[i, j].SetHCost(Chebychev(_nodeMap[i, j],finishNode));
                }
            }
        }


        private List<Node> LRTAVariosPasos(Node startNode, Node finishNode)
        {
            return null;
        }


        // LRTA Star
        private List<Node> LRTAStarUnPaso(Node startNode, Node finishNode)
        {
            SetCostsToStart();
            SetHeuristicOfEveryOne(finishNode);
            
            Node actualNode = new Node(startNode);
            List<Node> actualPath = new List<Node>{actualNode};
            
            while (!actualNode.Equals(finishNode))
            {
                List<Node> neighboursList = GetNeighboursList(actualNode);
                Node min = neighboursList[0];
                int minCost = Int32.MaxValue;
                foreach (var neighbour in neighboursList)
                {
                    if (1 + neighbour.GetHCost() < minCost)
                    {
                        min = neighbour;
                        minCost = 1 + neighbour.GetHCost();
                    }
                }
                actualNode.SetHCost(1 + min.GetHCost());
                actualNode = min;
                actualPath.Add(actualNode);
            }
            
            return actualPath;
        }

        public void ApplyLRTA(Cell startCell, Cell finishCell)
        {
            if (startCell != null && finishCell != null && startCell != finishCell && startCell.GetIsAllowedCell() &&
                finishCell.GetIsAllowedCell())
            {
                SetCostsToStart();
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                _path.Clear();
                _path = LRTAStarUnPaso(startNode, finishNode);
            }
        }


        // A STAR
        private void ApplyAStar(Cell startCell, Cell finishCell, int heuristic)
        {
            if (startCell != null && finishCell != null && startCell != finishCell && startCell.GetIsAllowedCell() &&
                finishCell.GetIsAllowedCell())
            {
                SetCostsToStart();
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                _path.Clear();
                _path = AStar(startNode, finishNode, heuristic);
            }
        }

        private List<Node> AStar(Node startNode, Node finalNode, int heuristic)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node> {startNode};

            startNode.SetGCost(0);
            startNode.SetHCost(HeuristicApply(startNode, finalNode, heuristic));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestCostFNode(openList);

                if (currentNode.GetCell().GetCoorX() == finalNode.GetCell().GetCoorX() &&
                    currentNode.GetCell().GetCoorZ() == finalNode.GetCell().GetCoorZ())
                {
                    return CalculatePath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);
                foreach (Node neighbourNode in GetNeighboursList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    int tentativeGCost = currentNode.GetGCost() + HeuristicApply(currentNode, neighbourNode, heuristic);

                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        neighbourNode.SetPreviousNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(HeuristicApply(neighbourNode, finalNode, heuristic));
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

        // Cost functions
        private int Manhattan(Node start, Node finish)
        {
            return Mathf.Abs(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX()) +
                   Mathf.Abs(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ());
        }

        private int Euclidean(Node start, Node finish)
        {
            // Se omite la raiz cuadrada ya que es costoso a nivel de calculo
            int x = Mathf.Abs((int) Mathf.Pow(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX(), 2));
            int z = Mathf.Abs((int) Mathf.Pow(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ(), 2));
            return x + z;
        }

        private int Chebychev(Node start, Node finish)
        {
            return Mathf.Max(Mathf.Abs(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX()),
                Mathf.Abs(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ()));
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
                    if (node.GetHCost() < Int32.MaxValue)
                    {
                        int coste = node.GetHCost();
                        cost = coste.ToString();
                    }

                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.blue;
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
        private void PathToString(List<Cell> lista)
        {
            for (var index = 0; index < lista.Count; index++)
            {
                var c = lista[index];
                Debug.Log("PASO " + index + ":" + c.GetCoorX() + " " + c.GetCoorZ());
            }
        }
    }
}