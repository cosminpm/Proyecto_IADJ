using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class Alignment: Align
{
    
    // Holds a list of potential targets
    [SerializeField] public List<Agent> targets;
 
    // Holds the threshold to take action
    [SerializeField] public float threshold = 10f;

    private Agent targetAux;
    // Por ahora esto funciona
    void Start()
    {
        targetAux = Target;
        GameObject prediccionGO = new GameObject("AuxAlignment");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = false;
        // invisible.InteriorAngle = targetAux.InteriorAngle;
        // invisible.ExteriorAngle = targetAux.ExteriorAngle;
        Target = invisible;
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        int count = 0;
        float heading = 0;
        float distance;
        Vector3 direction;

        foreach( var target in targets)
        {
            direction = target.Position - agent.Position;
            distance = Mathf.Abs(direction.magnitude);

            if (distance > threshold) continue;

            heading += target.Orientation;
            count++;
        }

        if ( count == 0) return steer;
        

        heading /= count;
        Target.Orientation = heading;

        return base.GetSteering(agent);

    }
}