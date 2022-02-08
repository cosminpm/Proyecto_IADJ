using UnityEngine;

public class VelocityMatching : SteeringBehaviour
{
    /*
     * Velocity Matching consiste en igualar la velocidad del usuario actual con la velocidad del agente seleccionado
     */
    [SerializeField] private Agent target;
    [SerializeField] private float timeToTarget = 0.1f;

    public Agent Target
    {
        get => target;
        set => target = value;
    }

    public void NewTarget(Agent a)
    {
        target = a;
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        steer.angular = 0;
        if (target == null)
        {
            steer.linear = Vector3.zero;
            return steer;
        }

        steer.linear = (target.Velocity - agent.Velocity) / timeToTarget;
        if (steer.linear.magnitude > agent.MaxAcceleration)
        {
            steer.linear.Normalize();
            steer.linear *= agent.MaxAcceleration;
        }

        return steer;
    }
}