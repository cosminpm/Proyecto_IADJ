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
 

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        Vector3 direction = Vector3.zero;
        float distance = 0;
        float strength = 0;

        // Loop through each target
        foreach(var target in targets){

                // check if the target is close
                direction = agent.Position - target.Position;
                distance = direction.magnitude;

                if (distance < threshold)
                {
                    // Calculate the strengh of repulsion
                    strength = Mathf.Min(coefficientK/(distance*distance), agent.MaxAcceleration);
                    direction.Normalize();
                    steer.linear += strength * direction;
                }
        }
        return steer;
    }
}