using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : Arrive {

    [SerializeField] float offSetDistance = 4f;

    [SerializeField] Agent targetHide;

    List<SphereCollider> listObjects;
    void Start(){

        GameObject go = new GameObject("auxHide");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;

        // Refactoring: nombre del atributo target ?
        Target = auxTarget;

        listObjects = new List<SphereCollider>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Hide");

        foreach (var g in gos )
        {
            listObjects.Add(g.GetComponent<SphereCollider>());
        }
    }



    public virtual void NewTarget(Agent t)
    {
    
    }

    private Vector3 getHidingPosition(SphereCollider go, Agent target ){
        float distanceFromObject = (go.transform.position - target.Position).magnitude;
        Vector3 vectorHiding = (go.transform.position - target.Position).normalized;
        vectorHiding *= go.radius + distanceFromObject;
        return vectorHiding + go.transform.position;
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        Vector3 hidingSpot = Vector3.zero;
        Vector3 bestHidingSpot = Vector3.zero;
        Vector3 direction = Vector3.zero;
        float distance = 0;
        float minDist = offSetDistance;

        foreach(var ob in listObjects){
            hidingSpot = getHidingPosition(ob, targetHide);
            direction = hidingSpot - agent.Position;
            distance = direction.magnitude;
            if ( distance < minDist)
            {
                minDist = distance;
                bestHidingSpot = hidingSpot;        
            }
        }

        if ( minDist == offSetDistance ) // No se ha encontrado ningun sitio para esconderse
        {   
            // TODO: Aqui se suponde que debe hacer un FACE?
            Debug.Log("No hay sitio donde esconderse");
            return steer;
        }
        else {
            Target.Position = bestHidingSpot;
            return base.GetSteering(agent); 
        }
    }


}