using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollowing : Seek {

    // Predicción máxima del movimiento
    [SerializeField] float offSetPrediction = 3f;
    [SerializeField] float offSetWall = 3f;
    
    [SerializeField] GameObject Wall;

    void Start(){

        GameObject go = new GameObject("auxWallFollowing");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;

        Target = auxTarget;
    }

    public override Steering GetSteering(Agent agent)
    {

        Steering steer = new Steering();

        // Futura posición del agente
        Vector3 posPrediction = agent.Position + agent.Velocity * offSetPrediction;

        // Calculamos el punto más cercano a la pared.
        Vector3 closestPoint = Wall.GetComponent<Collider>().ClosestPoint(posPrediction);

        // Vector director to wall
        Vector3 direction = closestPoint - posPrediction;
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