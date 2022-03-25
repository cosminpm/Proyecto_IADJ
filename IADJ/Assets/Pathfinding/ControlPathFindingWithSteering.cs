﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Grid;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Pathfinding
{
    public class ControlPathFindingWithSteering : MonoBehaviour
    {
        private Path _path;
        private PathFollowing _pathFollowing;
        private PathFinding _pathFinding;
        GridMap gridMap;

        public Color startColor = Color.cyan;
        public Color finishColor = Color.red;
        public bool drawColorPath, drawNumberPath;
        public int sizeOfTextPath = 10;

        public void Start()
        {
            _pathFinding = gameObject.AddComponent<PathFinding>();
            _pathFollowing = gameObject.AddComponent<PathFollowing>();
            _pathFollowing.weight = 1;

            _pathFollowing.path = gameObject.AddComponent<Path>();
            _path = _pathFollowing.path;

            _path.nodos = new List<Node>();
            gridMap = GameObject.Find("Controlador").GetComponent<GridMap>();
            
            drawColorPath = true;
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                Cell startCell = WorldToMap(transform.position);
                Cell finishCell = gridMap.CheckIfCellClicked(true);

                _path.nodos = new List<Node>();
                _pathFollowing.currentPos = 0;
                _pathFinding.ApplyLRTA(startCell, finishCell, ref _path.nodos);
            }
        }


        private void OnDrawGizmos()
        {
            DrawPath();
        }


        private void DrawPath()
        {
            if (_path != null)
            {
                int pathLen = _path.nodos.Count;
                for (var index = 0; index < _path.nodos.Count; index++)
                {
                    var c = _path.nodos[index];
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

        public Cell WorldToMap(Vector3 v)
        {
            foreach (var cell in gridMap.GetCellMap())
            {
                if (cell.CheckIfVector3InsideBox(v))
                {
                    return cell;
                }
            }
            throw new Exception("The player is not in the grid");
        }
        
    }
}