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


    // BORRAR
    private float radioExteriorInvisible = 3f;
    private float radioInteriorInvisible = 1f;

    private AgentInvisible auxTarget;

    public void Start(){

        GameObject go = new GameObject("auxWallInvisible");
        auxTarget = go.AddComponent<AgentInvisible>() as AgentInvisible;
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        auxTarget.ArrivalRadius = radioExteriorInvisible;
        auxTarget.InteriorRadius = radioInteriorInvisible;

    }




    public override Steering GetSteering(Agent agent){
        
        // TODO: Lo hago de la primera FORMA1: No teniendo en cuenta la velocidad, 

        // Construimos los bigotes
        Vector3 bigCentral = agent.Velocity.normalized;
        Vector3 bigIzquierdo = Quaternion.Euler(0,-anguloBigotes,0) * agent.Velocity.normalized;
        Vector3 bigDerecho = Quaternion.Euler(0,anguloBigotes,0) * agent.Velocity.normalized;

        RaycastHit hitCent,hitIzq,hitDer;

        // Detectamos las posibles colisiones. Para la posible trampa de la esquina en una posible colisión, le damos prioridad al bigote central.

                                                                                          // Colisión frontal-
        if (Physics.Raycast(agent.Position, bigCentral, out hitCent, lookAheadCentral))
        {
         
            auxTarget.Position = hitCent.point + hitCent.normal * avoidDistance;
            this.target = auxTarget;
            return base.GetSteering(agent);
            
        } else if (Physics.Raycast(agent.Position, bigIzquierdo, out hitIzq, lookAhead)) { // Colision bigote izquierdo

            auxTarget.Position = hitIzq.point + hitIzq.normal * avoidDistance;
            this.target = auxTarget;
            return base.GetSteering(agent);
            
        } else if (Physics.Raycast(agent.Position, bigDerecho, out hitDer, lookAhead)) { // Colision bigote derecho

            auxTarget.Position = hitDer.point + hitDer.normal * avoidDistance;
            this.target = auxTarget;
            return base.GetSteering(agent);

        }


        //return base.GetSteering(agent);
        
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
