using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class Cohesion: Arrive
{
    

    // Holds a list of potential targets
    [SerializeField] public List<Agent> targets;
 
    // Holds the threshold to take action
    [SerializeField] public float threshold = 0f;

    private Agent targetAux;

    void Start(){
        targetAux = Target;
        GameObject prediccionGO = new GameObject("AuxCohesion");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        invisible.GetComponent<AgentInvisible>().DrawGizmos = false;
        Target = invisible;
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        int count = 0;
        Vector3 centerOfMass = Vector3.zero;
        float distance = 0;
        Vector3 direction = Vector3.zero;

        foreach(var t in targets)
        {

            direction = t.Position - agent.Position;
            distance = Mathf.Abs(direction.magnitude);

            if ( distance > threshold)
            {
                centerOfMass += t.Position;
                count++;
            }
        }

        if ( count == 0) 
            return steer;
         
        centerOfMass /= count;
        Target.Position = centerOfMass;

        return base.GetSteering(agent);
    }
}