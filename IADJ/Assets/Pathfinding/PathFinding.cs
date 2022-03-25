using System;
using System.Collections.Generic;
using System.Globalization;
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
        public int heuristic = 2;
        public int radio = 3;
        public int maxCounterLRTA = 1000;
        
        // Private variables
        private int localSpaceMode = 0;
        private Node[,] _nodeMap;
        private int _xSize, _zSize;
        private List<Node> _path;

        private void Start()
        {
            _path = new List<Node>();
            InitializeNodeMap();
        }

        private void Update()
        {
            // Cell startCell = GetComponent<GridMap>().CheckIfCellClicked(Input.GetKeyUp(KeyCode.Alpha1));
            // Cell finishCell = GetComponent<GridMap>().CheckIfCellClicked(Input.GetKeyUp(KeyCode.Alpha2));
            //
            // List<Node> finalPath = new List<Node>(); 
            // ApplyLRTA(startCell, finishCell, ref finalPath);
        }

        private float HeuristicApply(Node startNode, Node finishNode, int heuristicApply)
        {
            switch (heuristicApply)
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

        private List<Node> GetNeighboursList(Node node, int heuristicApply)
        {
            List<Cell> neighbours = GameObject.Find("Controlador").GetComponent<GridMap>().GetAllNeighbours(node.GetCell(), heuristicApply).ToList();

            List<Node> result = new List<Node>();
            foreach (var neighbour in neighbours)
            {
                result.Add(RecoverNodeFromCell(neighbour));
            }

            return result;
        }

        private List<Node> GetNeighboursList(Node node)
        {
            return GetNeighboursList(node, 999);
        }


        public Node RecoverNodeFromCell(Cell cell)
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
                if (pathNodeList[index].GetFCost() + pathNodeList[index].GetGCost() <
                    lowestFNodeCost.GetFCost() + lowestFNodeCost.GetGCost())
                    lowestFNodeCost = pathNodeList[index];
            }

            return lowestFNodeCost;
        }

        private void InitializeNodeMap()
        {
            GridMap gridMap = GameObject.Find("Controlador").GetComponent<GridMap>();
            _xSize = gridMap.GetXSize();
            _zSize = gridMap.GetZSize();
            _nodeMap = new Node[_xSize, _zSize];
            SetCostsToStart();
        }

        private void SetCostsToStart()
        {
            GridMap gridMap = GameObject.Find("Controlador").GetComponent<GridMap>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _nodeMap[i, j] = new Node(gridMap.GetCellMap()[i, j], Mathf.Infinity, Mathf.Infinity, null);
                    _nodeMap[i, j].SetFCost(Mathf.Infinity);
                }
            }
        }

        // Functions of Finding Path
        private void SetHeuristicOfEveryOne(Node finishNode, int heuristicCost)
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _nodeMap[i, j].SetHCost(HeuristicApply(_nodeMap[i, j], finishNode, heuristicCost));
                    if (_nodeMap[i, j].Equals(finishNode))
                    {
                        _nodeMap[i, j].SetHCost(0f);
                    }
                }
            }
        }

        // LRTA Star
        private List<Node> LRTAVariosPasos(Node startNode, Node finalNode, int heuristicApply, ref List<Node> finalPath)
        {
            SetHeuristicOfEveryOne(finalNode, heuristicApply);
            Node currentNode = new Node(startNode);
            finalPath.Add(startNode);

            int contador = 0;

            while (!currentNode.Equals(finalNode))
            {
                List<Node> localSpace = GetLocalSpace(currentNode, finalNode);

                if (!localSpace.Contains(currentNode))
                    UpdateValuesLocalSpace(localSpace, heuristicApply);

                float minCost = Mathf.Infinity;
                List<Node> neighbours = GetNeighboursList(currentNode, heuristicApply);
                Node minNode = neighbours[0];
                foreach (var neighbour in neighbours)
                {
                    if (neighbour.GetHCost() < minCost)
                    {
                        minCost = neighbour.GetHCost();
                        minNode = neighbour;
                    }
                }

                currentNode = new Node(minNode);
                finalPath.Add(currentNode);

                contador += 1;
                if (contador > maxCounterLRTA)
                {
                    return finalPath;
                }
            }
            return finalPath;
        }

        private void UpdateValuesLocalSpace(List<Node> localSpace, int costHeuristic)
        {
            List<Node> copyLocalSpace = new List<Node>();
            foreach (var node in localSpace)
                copyLocalSpace.Add(new Node(node));

            foreach (var node in copyLocalSpace)
            {
                node.SetTempCost(node.GetHCost());
                node.SetMaxCost(Mathf.NegativeInfinity);
                node.SetHCost(Mathf.Infinity);
            }

            // While Infinit values exists in the HCost
            while (copyLocalSpace.Count > 0)
            {
                foreach (var node in copyLocalSpace)
                {
                    float minCost = Mathf.Infinity;
                    foreach (var neighbour in GetNeighboursList(node))
                    {
                        float hCost = neighbour.GetHCost() + HeuristicApply(neighbour, node, costHeuristic);
                        if (hCost < minCost)
                            minCost = hCost;
                    }

                    float maxCost = Mathf.Max(minCost, node.GetTempCost());
                    node.SetMaxCost(maxCost);
                }

                Node maxNode = copyLocalSpace[0];
                float maxValue = Mathf.NegativeInfinity;
                foreach (var node in copyLocalSpace)
                {
                    if (node.GetMaxCost() > maxValue)
                    {
                        maxValue = node.GetMaxCost();
                        maxNode = node;
                    }
                }

                // Recover node from not the copy and set it cost
                RecoverNodeFromCell(maxNode.GetCell()).SetHCost(maxNode.GetMaxCost());
                maxNode.SetHCost(maxNode.GetMaxCost());
                // Set the cost in the local space
                foreach (var node in localSpace)
                    node.SetMaxCost(Mathf.Infinity);
                foreach (var node in copyLocalSpace)
                    node.SetMaxCost(Mathf.Infinity);

                copyLocalSpace.Remove(maxNode);
            }
        }

        private List<Node> GetLocalSpace(Node startNode, Node finishNode)
        {
            switch (localSpaceMode)
            {
                case 0:
                    return GetLocalSpaceRadio(startNode, finishNode);

                // No se ha podido implementar
                // case 1:
                //     return GetLocalSpaceAStar(startNode, finishNode);

                default:
                    return GetLocalSpaceRadio(startNode, finishNode);
            }
        }


        private List<Node> GetLocalSpaceAStar(Node startNode, Node finishNode)
        {
            List<Node> localSpace = AStar(startNode, finishNode);
            return localSpace;
        }


        private List<Node> GetLocalSpaceRadio(Node startNode, Node finishNode)
        {
            List<Node> localSpace = GetLSRadioRecursive(startNode, radio);
            localSpace = localSpace.Distinct().ToList();
            localSpace.Remove(finishNode);
            return localSpace;
        }


        private List<Node> GetLSRadioRecursive(Node startNode, int radioNeighbours)
        {
            List<Node> neighboursList = GetNeighboursList(startNode);
            List<Node> localSpace = new List<Node>();
            localSpace.AddRange(neighboursList);

            int count = 1;
            while (count < radioNeighbours)
            {
                foreach (var neighbour in neighboursList)
                    localSpace.AddRange(GetLSRadioRecursive(neighbour, radioNeighbours - 1));
                count += 1;
            }

            return localSpace;
        }


        private List<Node> LRTAStarUnPaso(Node startNode, Node finishNode, int heuristicApply)
        {
            SetCostsToStart();
            SetHeuristicOfEveryOne(finishNode, heuristicApply);
            Node actualNode = new Node(startNode);

            List<Node> actualPath = new List<Node> {actualNode};
            while (!actualNode.Equals(finishNode))
            {
                List<Node> neighboursList = GetNeighboursList(actualNode);
                Node min = neighboursList[0];
                float minCost = Mathf.Infinity;
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

        public void ApplyLRTA(Cell startCell, Cell finishCell, ref List<Node> finalPath)
        {
            if (CellIsGood(startCell) && CellIsGood(finishCell) && !startCell.Equals(finishCell))
            {
                SetCostsToStart();
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                _path.Clear();
                _path = LRTAVariosPasos(startNode, finishNode, heuristic, ref finalPath);
            }
        }


        private bool CellIsGood(Cell node)
        {
            return node != null && node.GetIsAllowedCell();
        }

        // A STAR
        private void ApplyAStar(Cell startCell, Cell finishCell, int heuristicCost)
        {
            if (startCell != null && finishCell != null && startCell != finishCell && startCell.GetIsAllowedCell() &&
                finishCell.GetIsAllowedCell())
            {
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                _path.Clear();
                _path = AStar(startNode, finishNode);
            }
        }

        private List<Node> AStar(Node startNode, Node finalNode)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node> {startNode};

            startNode.SetGCost(0);
            startNode.SetHCost(HeuristicApply(startNode, finalNode, heuristic));
            startNode.CalculateFCost();
            
            int count = 0;

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestCostFNode(openList);

                if (currentNode.Equals(finalNode))
                {
                    return CalculatePath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);
                foreach (Node neighbourNode in GetNeighboursList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    float tentativeGCost =
                        currentNode.GetGCost() + HeuristicApply(currentNode, neighbourNode, heuristic);

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

                count += 1;
            }

            return null;
        }

        // Cost functions
        private float Manhattan(Node start, Node finish)
        {
            return Mathf.Abs(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX()) +
                   Mathf.Abs(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ());
        }

        private float Euclidean(Node start, Node finish)
        {
            // Se omite la raiz cuadrada ya que es costoso a nivel de calculo
            int x = Mathf.Abs((int) Mathf.Pow(start.GetCell().GetCoorX() - finish.GetCell().GetCoorX(), 2));
            int z = Mathf.Abs((int) Mathf.Pow(start.GetCell().GetCoorZ() - finish.GetCell().GetCoorZ(), 2));
            return Mathf.Sqrt(x + z);
        }

        private float Chebychev(Node start, Node finish)
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
                    if (node.GetHCost() < Mathf.Infinity)
                    {
                        float coste = node.GetHCost();
                        cost = coste.ToString(CultureInfo.CurrentCulture);
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