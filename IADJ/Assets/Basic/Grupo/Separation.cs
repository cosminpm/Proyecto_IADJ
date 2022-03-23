using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class Separation: SteeringBehaviour
{
    

    // Holds a list of potential targets
    [SerializeField] public List<Agent> targets;
 
    // Holds the threshold to take action
    [SerializeField] public float threshold = 1f;

     // Holds the constan coefficient of decay for the inverse square law force
    [SerializeField] public float coefficientK = 1f;

    // Holds the maximun acceleration of the character
    [SerializeField] public float maxAcceleration;
 
    // Por ahora esto funciona
    public virtual void NewTarget(Agent t)
    {
        
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        Vector3 direction;
        float distance;
        float strength;

        // Loop through each target
        foreach(var target in targets){

                // check if the target is close
                direction = target.Position - agent.Position;
                distance = direction.magnitude;

                if (distance < threshold)
                {
                    // Calculate the strengh of repulsion
                    strength = Mathf.Min(coefficientK/(distance*distance), agent.MaxAcceleration);
                    steer.linear += strength * direction.normalized;
                }
        }
        return steer;
    }
}