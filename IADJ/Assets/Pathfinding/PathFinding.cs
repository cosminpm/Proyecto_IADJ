using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Global;
using Grid;
using InfluenceMap;
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
        public int heuristic = 0;

        // Private variables
        private Node[,] _nodeMap;
        private int _xSize, _zSize;
        private List<Node> _path;

        // Modo táctico
        public bool tactic = true;
        public NPC npc;

        private void Start()
        {
            _path = new List<Node>();
            InitializeNodeMap();
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
            List<Cell> neighbours = GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>()
                .GetAllNeighbours(node.GetCell(), heuristicApply).ToList();

            List<Node> result = new List<Node>();
            foreach (var neighbour in neighbours)
            {
                result.Add(RecoverNodeFromCell(neighbour));
            }

            return result;
        }

        private List<Node> GetNeighboursList(Node node)
        {
            return GetNeighboursList(node, heuristic);
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
                // if (pathNodeList[index].GetFCost() + pathNodeList[index].GetGCost() <
                //     lowestFNodeCost.GetFCost() + lowestFNodeCost.GetGCost())
                //     lowestFNodeCost = pathNodeList[index];

                if (pathNodeList[index].GetFCost() < lowestFNodeCost.GetFCost())
                    lowestFNodeCost = pathNodeList[index];
            }

            return lowestFNodeCost;
        }

        private void InitializeNodeMap()
        {
            GridMap gridMap = GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>();
            _xSize = gridMap.GetXSize();
            _zSize = gridMap.GetZSize();
            
            _nodeMap = new Node[_xSize, _zSize];
            SetCostsToStart();
        }

        private void SetCostsToStart()
        {
            GridMap gridMap = GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _nodeMap[i, j] = new Node(gridMap.GetCellMap()[i, j], Mathf.Infinity, Mathf.Infinity, null);
                    _nodeMap[i, j].SetFCost(Mathf.Infinity);
                }
            }
        }

        public void ApplyAStar(Cell startCell, Cell finishCell, ref List<Node> path)
        {
            if (CellIsGood(startCell) && CellIsGood(finishCell) && !startCell.Equals(finishCell))
            {
                SetCostsToStart();
                Node startNode = RecoverNodeFromCell(startCell);
                Node finishNode = RecoverNodeFromCell(finishCell);
                AStar(startNode, finishNode, ref path);
            }
        }


        private bool CellIsGood(Cell node)
        {
            return node != null && node.GetIsAllowedCell();
        }

        private List<Node> AStar(Node startNode, Node finalNode, ref List<Node> path)
        {
            List<Node> closedList = new List<Node>();
            
            startNode.SetGCost(0);
            startNode.SetHCost(HeuristicApply(startNode, finalNode, heuristic));
          //  startNode.CalculateFCost();
            
            startNode.SetFCost(startNode.GetHCost()+startNode.GetGCost());

            List<Node> openList = new List<Node> {startNode};

           
            while (openList.Count > 0)
            {
                Node currentNode = GetLowestCostFNode(openList);
                if (currentNode.Equals(finalNode))
                {
                    List<Node> calculatedPath =  CalculatePath(currentNode);
                    path = calculatedPath;
                    return calculatedPath;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);
                foreach (Node neighbourNode in GetNeighboursList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                  //   float tentativeGCost =  currentNode.GetGCost() + HeuristicApply(currentNode, neighbourNode, heuristic);
                    float h = CalculateCost(currentNode, neighbourNode);
                   // Debug.Log("La coste "+coste);
                    // Debug.Log("El coste es "+ coste);
                   // Debug.Log("El nodo actual es "+currentNode+ " y el nodo vecino es el "+neighbourNode );

                    float tentativeGCost = currentNode.GetGCost() + h;

                    
                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        neighbourNode.SetPreviousNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(HeuristicApply(currentNode, neighbourNode, heuristic));
                        neighbourNode.SetFCost(neighbourNode.GetHCost()+neighbourNode.GetGCost());
                       // neighbourNode.SetFCost(neighbour.GetFCost())
                   //     neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }

        private float CalculateCost(Node currentNode, Node neighbourNode){

            float cost = 0;

            if (tactic){
                Cell neighbourCell = neighbourNode.GetCell();
                // Coste base del terreno + Coste de movimiento de la unidad en ese terreno
                
                // float costInfluence = GameObject.Find("GridController").GetComponent<InfluenceMap.InfluenceMap>().GetValueOfInfluence(neighbourCell.GetCoorX(),neighbourCell.GetCoorZ(), npc.GetUnitTeam());
                // float costeTerrain = neighbourCell.GetTerrainCost() *
                //                      npc.Unit.GetMovementCost(neighbourCell.GetTipoTerreno());
                // float costVisibility = GameObject.Find("GridController").GetComponent<VisibilityMap>()
                //     .GetVisibilityEnem(neighbourCell.GetCoorX(), neighbourCell.GetCoorZ(), npc.GetUnitTeam());
                // cost = costeTerrain+ costInfluence + costVisibility;


                float costeTerrain = neighbourCell.GetTerrainCost() *
                                     npc.Unit.GetMovementCost(neighbourCell.GetTipoTerreno());
                

                 cost = costeTerrain;
                
                // Influencia
            } else {
                cost = HeuristicApply(currentNode, neighbourNode, heuristic);
            }
            return cost;
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
                    if (node.GetFCost() < Mathf.Infinity)
                    {
                        float coste = node.GetFCost();
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