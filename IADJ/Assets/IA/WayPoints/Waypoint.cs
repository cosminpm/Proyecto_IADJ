using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Global;

// Manejador de combate
public class Waypoint : MonoBehaviour
{
    public enum TypeWayPoint {

        BaseRoja,
        BaseAzul,
        BaseEnemigaRoja,
        BaseEnemigaAzul,
        PatrullaRoja,
        PatrullaAzul
    }

    [SerializeField] public TypeWayPoint typeWaypoint;

    public float winningPercentage;

     // Distancia mínima a la que debe estar el npc para estar dentro del waypoint
    public float distanceMinWaypoint;

    [SerializeField] public Transform[] waypointPos;
    

    void Start() {
        if (typeWaypoint == TypeWayPoint.BaseEnemigaAzul || typeWaypoint == TypeWayPoint.BaseEnemigaAzul)
            winningPercentage = 0;
    }

    void Update() {}


    public int GetWaypointPosLenght()
    {
        return waypointPos.Length;
    }
}

   