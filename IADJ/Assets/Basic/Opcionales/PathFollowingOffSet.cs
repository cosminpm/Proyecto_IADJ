using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingOffSet : Seek {

/*
    // Predicción máxima del movimiento
    [SerializeField] float predictTime = 1f;

    [SerializeField] Path path;
    
    private Vector3 targetParam;
    // Mantiene la posicióna actual en el path
    private Vector3 currentParam;

    private float pathOffset;

    void Start(){
        GameObject go = new GameObject("auxFollowing");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        // Refactoring: nombre del atributo target ?
        Target = auxTarget;  
    }
*/

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        /*
        // Futura posición del agente
        Vector3 futurePos = agent.Position + agent.Velocity* predictTime;

        Vector3 currentParam = path.getParam(futurePos, currentPos);


        // Offset it
        Vector3 targetParam = currentParam + pathOffset;

        // get te target position
        Target.Position = path.getPosition(targetParam);
        */
        return base.GetSteering(agent);

        
    }
}