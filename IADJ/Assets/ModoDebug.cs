using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global;
using Pathfinding;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ModoDebug : MonoBehaviour
{

    
    public bool mostrarWaypoints;

    public bool mostrarEstados;

    public bool mostrarPath;
    // Start is called before the first frame update

    public bool modoTactico;

    void Start()
    {
        modoTactico = true;
    }

    // Update is called once per frame
    void Update()
    {
        MostrarEstados();
        MostrarPath();
        ActivarModoTactico();
    }

    public void ActivarModoTactico()
    {
        List<GameObject> soldados =  GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL).ToList();
        soldados.AddRange(GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO).ToList());
        
        foreach (var s in soldados)
        {
            s.GetComponent<PathFinding>().tactic = modoTactico;
            //    s.GetComponent<ControlPathFindingWithSteering>().ClearPath();
             //   s.GetComponent<ControlPathFindingWithSteering>().ReCalculatePath();
        }

        
    }
    
    public void MostrarWaipoints()
    {
        GameObject parent = GameObject.Find("Waypoints");
        foreach (Transform child in parent.transform)
        {
            Debug.Log(child.gameObject.GetComponent<Waypoint>().typeWaypoint);
            Waypoint.TypeWayPoint tipo = child.gameObject.GetComponent<Waypoint>().typeWaypoint;
            if (tipo == Waypoint.TypeWayPoint.BaseAzul || tipo == Waypoint.TypeWayPoint.PatrullaAzul || tipo == Waypoint.TypeWayPoint.BaseEnemigaAzul)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;
            foreach (Transform c in child)
            {
                Gizmos.DrawSphere(c.gameObject.transform.position, 2f);
            }
        }
    }
    
    public void MostrarPath()
    {
        List<GameObject> soldados =  GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL).ToList();
        soldados.AddRange(GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO).ToList());
        foreach (var s in soldados)
        {
            s.GetComponent<ControlPathFindingWithSteering>().drawColorPath = mostrarPath;
        }
    }
    

    public void MostrarEstados()
    {
        List<GameObject> soldados =  GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL).ToList();
        soldados.AddRange(GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO).ToList());
        foreach (var s in soldados)
        {
            s.transform.Find("Canvas").gameObject.SetActive(mostrarEstados);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (mostrarWaypoints)
        {
            MostrarWaipoints();
        }
    }
}
