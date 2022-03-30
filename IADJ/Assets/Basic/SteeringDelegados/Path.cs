using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class Path : MonoBehaviour
{
    public List<Node> nodos;
    [SerializeField] public float radioNodo;
    
    public void AddNodo(Node nodo)
    {
        nodos.Add(nodo);
    }

    public List<Node> GetNodos()
    {
        return nodos;
    }

    public int Length()
    {
        return nodos.Count;
    }


    // Devuelve la posición del nodo en el camino hacia la que tiene que ir target 
    public Vector3 GetPosition(int nodoPosicion)
    {
        // Devolvemos la posición actual si no hay camino.
        if (nodos.Count == 0)
            return transform.position;


        // Devolvemos el último nodo el camino, si hemos llegado la final
        if (nodoPosicion >= nodos.Count)
            return nodos[nodos.Count - 1].GetCell().GetCenter();
        
        // TODO:Por errores
        if (nodoPosicion < 0)
            return nodos[0].GetCell().GetCenter();
        
        // Devolve
        return nodos[nodoPosicion].GetCell().GetCenter();
    }

    // Encuentra la posicion actual del nodo en el camino
    public int GetParam(Vector3 agentePosicion, int nodoActual)
    {
        // Devolvemos el nodo actual si no hay camino.
        if (nodos.Count <= 1)
            return nodoActual;


        // Si nos pasamos nos quedamos en el último nodo del cámino
        if (nodoActual >= nodos.Count)
            return nodos.Count - 1;

        // Ahora tenemos que calcular si nos vamos al nodo siguiente o no..
        return nodoActual;
    }

    public bool CondArrive(Vector3 agentePosicion, int nodoActual)
    {
        Debug.Log("NODO ACTUAL:"+nodoActual);
        Debug.Log("SIZ NODOS:"+nodos.Count);

        if (nodos.Count > 0)
        {
            Debug.Log("CELDA ACTUAL:"+nodos[nodoActual].GetCell());
            Debug.Log(nodos[nodoActual].GetCell().CheckIfVector3InsideBox(agentePosicion));
            return nodos[nodoActual].GetCell().CheckIfVector3InsideBox(agentePosicion);
        }
        Debug.Log("HE SALIDO");  
        return false;
    }
}