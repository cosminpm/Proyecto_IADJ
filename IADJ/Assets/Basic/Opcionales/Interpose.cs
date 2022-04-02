using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpose : Arrive {

    [SerializeField] Agent agentA;
    [SerializeField] Agent agentB;


    void Start(){

        GameObject go = new GameObject("auxInterpose");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        auxTarget.Position = Vector3.zero;
        // Refactoring: nombre del atributo target ?
       
        Target = auxTarget;
    }


    public virtual void NewTarget(Agent t)
    {
    
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steering = new Steering();

        // Calculamos la mitad entre los dos agentes
        Vector3 midPoint = (agentA.Position + agentB.Position) / 2;

        Debug.Log(midPoint);

        // Calculamos el tiempo que se tarda en ir al punto medio
        Vector3 direction = midPoint - agent.Position;
        float distance = direction.magnitude;
        float timeToReachMidPoint = distance / agent.Velocity.magnitude;

        // Predecimos la nueva posición de los agentes asumiendo el tiempo anterior
        Vector3 aPos = agentA.Position + agentA.Velocity * timeToReachMidPoint;
        Vector3 bPos = agentB.Position + agentB.Velocity * timeToReachMidPoint;

        // Calculamos el punto medio de la predicción
        midPoint = ( aPos+bPos)/2;

        // Asignamos al allign su nuevo target.
        
        Target.Position = midPoint;

        Debug.Log("La posicion del target es "+ Target.Position);
         
        return base.GetSteering(agent);
    }
}