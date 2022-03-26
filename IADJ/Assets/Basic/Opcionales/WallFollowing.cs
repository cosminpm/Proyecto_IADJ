using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollowing : Seek {

    // Predicción máxima del movimiento
    [SerializeField] float offSetPrediction = 3f;
    [SerializeField] float offSetWall = 3f;
    

    private List<Collider> wallsCollider;  

    void Start(){

        GameObject go = new GameObject("auxWallFollowing");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;

        // Refactoring: nombre del atributo target ?
        Target = auxTarget;
        // Inicializamos las paredes

        wallsCollider = new List<Collider>();
        // Obtenemos todas las paredes
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Wall");

        foreach( var w in gos)
        {
            wallsCollider.Add(w.GetComponent<Collider>());
        }
    }


    public override Steering GetSteering(Agent agent)
    {

        Steering steer = new Steering();

        Vector3 posPrediction = agent.Position + agent.Velocity * offSetPrediction;

        float minDistance = Mathf.Infinity;

        Collider wall = null;
        Vector3 point = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 closestPoint = Vector3.zero;

        // Recorremos las paredes en busca del punto más cercano a la futura pos del agent
        foreach( var wallAux in wallsCollider)
        {
            closestPoint = wallAux.GetComponent<Collider>().ClosestPoint(posPrediction);
            direction = posPrediction - closestPoint;

            // Encuentro la wall que esté más cercana al agente.
            if ( direction.magnitude < minDistance)
            {
                wall = wallAux;
                point = closestPoint;
            }
        }


        // Ahora comprobamos que no hay una posible colisión con la wall

        // Vector director to wall
        direction = closestPoint - posPrediction;
        Vector3 positionFinal = Vector3.zero;

        RaycastHit hit;
        // Si hay colisión, sacamos la normal del punto de colisión.
        if (Physics.Raycast(posPrediction, direction, out hit))
        {
            positionFinal = hit.point + hit.normal * offSetWall;

        } else {
            positionFinal = closestPoint * offSetWall;
        }

        positionFinal.y = 0;
        Target.Position = positionFinal;

        return base.GetSteering(agent);
    }
}