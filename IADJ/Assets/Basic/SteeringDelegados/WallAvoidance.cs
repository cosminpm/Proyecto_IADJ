using System;
using UnityEditor;
using UnityEngine;



public class WallAvoidance : Seek
{
   
    [SerializeField] public float avoidDistance = 15f; 
    [SerializeField] public float lookAhead = 5f;
    [SerializeField] public float lookAheadCentral = 10f;

    [Tooltip("Dibujar todo")] [SerializeField]  
    protected bool _drawGizmos;
    [SerializeField] private float _anchuraLinea = 1.5f;
    [SerializeField] private float anguloBigotes = 45f;


    public void Start(){


    }


    public override Steering GetSteering(Agent agent){
/*
        // TODO: Lo hago de la primera FORMA1: No teniendo en cuenta la velocidad, 

        Vector3 bigotes = this.Velocity.normalized * lookAhead;

        // Construimos los bigotes
        Vector3 bigCentral = this.Velocity.normalized * lookAheadCentral;
        Vector3 bigIzquierdo = Quaternion.Euler(0,-anguloBigotes,0) * lookAheadCentral;
        Vector3 bigDerecho = Quaternion.Euler(0,anguloBigotes,0) *lookAheadCentral;

        Raycast hitCen,hitIzq,hitDer;

        // Detectamos las posibles colisiones. Para la posible trampa de la esquina en una posible colisión, le damos prioridad al bigote central.

        // Colisión frontal-
        if (Physics.Raycast(agent.Position, bigCentral, out hitCen, lookAheadCentral))
        {
            

        } else if (Physics.Raycast(agent.Position, bigIzquierdo, out hitIzq, lookAhead)) { // Colision bigote izquierdo
          
        } else if (Physics.Raycast(agent.Position, bigDerecho, out hitDer, lookAhead)) { // Colision bigote derecho
       
        }


        return base.GetSteering(agent);
        */
        return new Steering();
    }


    public void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Vector3 position = transform.position;
            Vector3 forward = transform.forward;

            // Bigote Izquierdo
            Handles.color = Color.blue;
            Vector3 izquierdo = Quaternion.Euler(0,-anguloBigotes,0) * forward;
            Handles.DrawLine(position, position + izquierdo * lookAhead,_anchuraLinea);

            // Bigote Derecho
            Handles.color = Color.blue;
            Vector3 derecho = Quaternion.Euler(0,anguloBigotes,0) * forward;
            Handles.DrawLine(position, position + derecho* lookAhead,_anchuraLinea);

            // Bigote Central
            Handles.color = Color.red;
            Handles.DrawLine(position, position + forward * lookAheadCentral,_anchuraLinea);
        }
    }



}
