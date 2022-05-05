﻿using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using Grid;
using UnityEditor;
using UnityEngine;

namespace InfluenceMap
{
    public class InfluenceMap:MonoBehaviour
    {
        private GridMap _gridMap;
        private InfluenceNode[,] _influenceNodes;
        private int _xSize, _zSize;
        private Dictionary<GameObject,Cell> _allPlayers;

        public bool drawInfluenceMapNode;

        private void Start()
        {
            _gridMap = GetComponent<GridMap>();
            _allPlayers = new Dictionary<GameObject, Cell>();
            CreateInfluenceMap();
            AddAllPlayers();
            UpdateValuesInfluence();
        }

        private void UpdateMap()
        {
            UpdatePositionPlayers();
            UpdateValuesInfluence();
            DecreaseInfluence();
        }
        private void Update()
        {
            UpdateMap();
            //InvokeRepeating("UpdateMap", 1f, 1f);
        }
        
        private void AddAllPlayers()
        {
            GameObject[] blueTeam = GetBlueTeamObjects();
            GameObject[] redTeam = GetRedTeamObjects();
            foreach (var e in blueTeam)
                _allPlayers.Add(e,_gridMap.WorldToMap(e.transform.position));
            foreach (var e in redTeam)
                _allPlayers.Add(e,_gridMap.WorldToMap(e.transform.position));
        }

        public void UpdatePositionPlayers()
        {
            foreach (var e in _allPlayers.Keys.ToList())
            {
                _allPlayers[e] = _gridMap.WorldToMap(e.transform.position);
            }
        }

        public void DecreaseInfluence()
        {
            foreach (var node in _influenceNodes)
            {
               node.SetValueOfInfluence(node.GetValueInfluence() - GlobalAttributes.CONSTANT_DECRASING_VALUE);
            }
        }
        
        public void UpdateValuesInfluence()
        {
            foreach (var e in _allPlayers)
            {
                UpdateFilterInfluences(e.Value.GetCoorX(), e.Value.GetCoorZ(), 10, TransformTagToInt(e.Key.tag), 10);
            }
        }

        private int TransformTagToInt(String tag)
        {
            if (tag == GlobalAttributes.TAG_EQUIPO_AZUL)
                return GlobalAttributes.CELDA_INFLUENCIA_AZUL;
            if (tag == GlobalAttributes.TAG_EQUIPO_ROJO)
                return GlobalAttributes.CELDA_INFLUENCIA_ROJO;
            return GlobalAttributes.CELDA_INFLUENCIA_NADIE;
        }

        private void CreateInfluenceMap()
        {
            _xSize = _gridMap.GetXSize();
            _zSize = _gridMap.GetZSize();
            _influenceNodes = new InfluenceNode[_xSize, _zSize];

            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _influenceNodes[i,j] = new InfluenceNode();
                }
            }
        }
        
        // For one concrete value
        private void UpdateOneValueOfInfluence(int i, int j, float valueOfInfluence, int teamInfluence, int radio)
        {
            _influenceNodes[i,j].SetNewInfluence(teamInfluence, valueOfInfluence);
        }

        // For multiple values
        private void UpdateFilterInfluences(int iIndex, int jIndex, float valueOfInfluence, int teamInfluence, int radio)
        {
            int xStart = iIndex - radio;
            int zStart = jIndex - radio;
            int xFinish = iIndex + radio;
            int zFinish = jIndex + radio;

            if (xStart < 0)
                xStart = 0;
            if (zStart < 0)
                zStart = 0;
            if (xFinish >= _xSize)
                xFinish = _xSize - 1;
            if (zFinish >= _zSize)
                zFinish = _zSize - 1;

            for (int i = xStart; i <= xFinish; i++)
            {
                for (int j = zStart; j <= zFinish; j++)
                {
                    float distance = Vector3.Distance(_gridMap.GetCellMap()[i, j].GetCenter(),
                                                      _gridMap.GetCellMap()[iIndex, jIndex].GetCenter());
                    if (distance > 0)
                    {
                        float value = valueOfInfluence / distance;
                        UpdateOneValueOfInfluence(i, j, value, teamInfluence,radio);
                    }
                    else
                    {
                        UpdateOneValueOfInfluence(i, j, GlobalAttributes.MAXIMUM_VALUE_INFLUENCE, teamInfluence,radio);
                    }
                }
            }

        }
        
        
        private GameObject[] GetBlueTeamObjects()
        {
            return GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL);
        }

        private GameObject[] GetRedTeamObjects()
        {
            return GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO);
        }
        
        
        private void DrawGridMap()
        {
            if (drawInfluenceMapNode)
            {
                for (int i = 0; i < _xSize; i++)
                {
                    for (int j = 0; j < _zSize; j++)
                    {
                    
                        _influenceNodes[i, j].DrawInfluenceNode(i, j);
                    }
                }
            }
           
        }

        private void OnDrawGizmos()
        {
            DrawGridMap();
        }
    }
}