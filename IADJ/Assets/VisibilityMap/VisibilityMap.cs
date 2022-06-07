using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using Grid;
using UnityEditor;
using UnityEngine;


public class VisibilityMap : MonoBehaviour
{
    private GridMap _gridMap;
    private VisibilityNode[,] _visibilityNodesRed;
    private VisibilityNode[,] _visibilityNodesBlue;

    public int radio = 20;
    private int _xSize, _zSize;
    private Dictionary<GameObject, Cell> _allPlayersRed;
    private Dictionary<GameObject, Cell> _allPlayersBlue;

    public bool drawVisibilityMapNode;

    private void Start()
    {
        _gridMap = GetComponent<GridMap>();
        _allPlayersRed = new Dictionary<GameObject, Cell>();
        _allPlayersBlue = new Dictionary<GameObject, Cell>();
        CreateInfluenceMap();
        AddPlayersToTheirTeam();
        UpdateValuesInfluence();
    }

    private void UpdateMap()
    {
        UpdatePositionPlayers();
        UpdateValuesInfluence();
    }

    private void Update()
    { 
        UpdateMap();
        DecreaseVisibility();
    }

    public void DecreaseVisibility()
    {
        foreach (var node in _visibilityNodesRed)
        {
            node.DecreaseVisibility(0.25f);
        }
        foreach (var node in _visibilityNodesBlue)
        {
            node.DecreaseVisibility(0.25f);
        }
    }
    
    private void AddPlayersToTheirTeam()
    {
        GameObject[] blueTeam = GetBlueTeamObjects();
        GameObject[] redTeam = GetRedTeamObjects();
        foreach (var e in blueTeam)
            _allPlayersBlue.Add(e, _gridMap.WorldToMap(e.transform.position));
        foreach (var e in redTeam)
            _allPlayersRed.Add(e, _gridMap.WorldToMap(e.transform.position));
    }

    public void UpdatePositionPlayers()
    {
        foreach (var e in _allPlayersRed.Keys.ToList())
        {
            _allPlayersRed[e] = _gridMap.WorldToMap(e.transform.position);
        }
        foreach (var e in _allPlayersBlue.Keys.ToList())
        {
            _allPlayersBlue[e] = _gridMap.WorldToMap(e.transform.position);
        }
        
    }

    public void UpdateValuesInfluence()
    {
        foreach (var e in _allPlayersRed)
        {
            // TODO SEGUN TIPO DE JUGADOR ASIGNAR UN VALOR O OTRO DE INFLUENCIA Y RADIO
            // TODO FUNCION PASADO OBJECT ASIGNAR INFLUENCIA Y RADIO
            UpdateVisibility(e.Value.GetCoorX(), e.Value.GetCoorZ(), radio,GlobalAttributes.RED_TEAM);
        }

        foreach (var e in _allPlayersBlue)
        {
            // TODO SEGUN TIPO DE JUGADOR ASIGNAR UN VALOR O OTRO DE INFLUENCIA Y RADIO
            // TODO FUNCION PASADO OBJECT ASIGNAR INFLUENCIA Y RADIO
            UpdateVisibility(e.Value.GetCoorX(), e.Value.GetCoorZ(), radio, GlobalAttributes.BLUE_TEAM);
        }
    }

    private void CreateInfluenceMap()
    {
        _xSize = _gridMap.GetXSize();
        _zSize = _gridMap.GetZSize();
        _visibilityNodesRed = new VisibilityNode[_xSize, _zSize];
        _visibilityNodesBlue = new VisibilityNode[_xSize, _zSize];
        for (int i = 0; i < _xSize; i++)
        {
            for (int j = 0; j < _zSize; j++)
            {
                _visibilityNodesRed[i, j] = new VisibilityNode();
                _visibilityNodesBlue[i, j] = new VisibilityNode();
            }
        }
    }

    // For one concrete value
    private void UpdateOneValueOfInfluence(int i, int j, int valueOfInfluence, int team)
    {
        if (team == GlobalAttributes.RED_TEAM)
            _visibilityNodesRed[i, j].SetNewInfluence(valueOfInfluence);
        else if (team == GlobalAttributes.BLUE_TEAM)
        {
            _visibilityNodesBlue[i, j].SetNewInfluence(valueOfInfluence);
        }
    }

    // For multiple values
    private void UpdateVisibility(int iIndex, int jIndex, int r, int team)
    {
        int xStart = iIndex - r;
        int zStart = jIndex - r;
        int xFinish = iIndex + r;
        int zFinish = jIndex + r;

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
                UpdateOneValueOfInfluence(i, j, GlobalAttributes.HAS_VISIBILITY, team);
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

    public float GetVisibilityEnem(int i, int j, int team)
    {
        if (team == GlobalAttributes.RED_TEAM)
            return _visibilityNodesBlue[i, j].GetValueVisibility();
        if (team == GlobalAttributes.BLUE_TEAM)
            return _visibilityNodesRed[i, j].GetValueVisibility();
        return 0;
    }

    private void DrawVisibilityMap()
    {
        if (drawVisibilityMapNode)
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _zSize; j++)
                {
                    _visibilityNodesRed[i, j].DrawVisibilityNodeRed(i, j);
                    _visibilityNodesBlue[i, j].DrawVisibilityNodeBlue(i, j);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawVisibilityMap();
    }
}