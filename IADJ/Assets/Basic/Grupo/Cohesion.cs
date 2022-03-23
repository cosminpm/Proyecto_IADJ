using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class Cohesion: Align
{
    

    // Holds a list of potential targets
    [SerializeField] public List<Agent> targets;
 
    // Holds the threshold to take action
    [SerializeField] public float threshold = 0f;

    // Por ahora esto funciona
    public virtual void NewTarget(Agent t)
    {
        
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        int count = 0;
        Vector3 centerOfMass = Vector3.zero;
        float distance;
        Vector3 direction;

        foreach(var target in targets)
        {
            direction = target.Position - agent.Position;
            distance = Mathf.Abs(direction.magnitude);

            if ( distance > threshold) continue;

            centerOfMass += target.Position;
            count++;
        }

        if ( count == 0)
        {
            Steering steering = new Steering();
            return steering;
        }
            
        centerOfMass /= count;
       // this.Target.Position = centerOfMass;

        return base.GetSteering(agent);
    }
}