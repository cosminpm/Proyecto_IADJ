using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Path : MonoBehaviour
{
    [SerializeField] public List<GameObject> nodos = new List<GameObject>();
    [SerializeField] public float radioNodo;
         
    public void addNodo(GameObject nodo) {      
            nodos.Add(nodo);
    }

    public List<GameObject> getNodos(){
        return nodos;
    }

    public int length(){
        return nodos.Count;
    }


    // Devuelve la posición del nodo en el camino hacia la que tiene que ir target 
    public Vector3 GetPosition(int nodoPosicion){


        // Devolvemos la posición actual si no hay camino.
        if (nodos.Count == 0){
            return this.transform.position;
        
        }

        // Devolvemos el último nodo el camino, si hemos llegado la final
        if (nodoPosicion >= nodos.Count){
            return nodos[nodos.Count - 1].transform.position;
        }

        // TODO:Por errores
        if (nodoPosicion < 0){
            return nodos[0].transform.position;
        }

        // Devolve
        return nodos[nodoPosicion].transform.position;


    }

    // Encuentra la posicion actual del nodo en el camino
    public int GetParam( Vector3 agentePosicion, int nodoActual){

        // Devolvemos el nodo actual si no hay camino.
        if ( nodos.Count <= 1){
            return nodoActual;
        }

        // Si nos pasamos nos quedamos en el último nodo del cámino
        if ( nodoActual >= nodos.Count){
            return nodos.Count - 1;
        }

        // Ahora tenemos que calcular si nos vamos al nodo siguiente o no..
        

        return nodoActual;
    }

    public bool condArrive(Vector3 agentePosicion, int nodoActual){

        if  ((Vector3.Distance(agentePosicion,nodos[nodoActual].transform.position)) < radioNodo)
        {
        
            return true;
        }

        return false;
        
    }



}