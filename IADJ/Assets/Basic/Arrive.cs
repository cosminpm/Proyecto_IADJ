using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehaviour
{
    [SerializeField] private Agent target;

    [SerializeField] private float timeToTarget = 0.1f;

    // Declara las variables que necesites para este SteeringBehaviour
    public Agent Target
    {
        get => target;
        set => target = value;
    }

    public void NewTarget(Agent a)
    {
        target = a;
    }
        
    void Start()
    {
        nameSteering = "Arrive Steering";
    }
    public override Steering GetSteering(Agent agent)
    {
        // Construimos el nuevo steering
        Steering steer = new Steering();
        steer.angular = 0;

        if (target == null)
        {
            steer.linear = Vector3.zero;
            return steer;
        }
        
        // Obtenemos la dirección hacia el target
        Vector3 direction = target.Position - agent.Position;

        // Distancia que hay entre el agente y el target
        float distance = direction.magnitude;

        // Comprobamos si la distancia es menor que el radio del agente, en cuyo caso, pararíamos el agente.
        if (distance <= target.InteriorRadius)
        {
            steer.linear = -agent.Velocity;
            steer.angular = 0;
            return steer;
        }

        // Comprobamos si la distancia es menor que el radio exterior del agente, en cuyo caso, se reduciría la velocidad progresivamente.
        float speedToTarget;
        if (distance <= target.ArrivalRadius)
        {
            speedToTarget = agent.MaxSpeed * distance / target.ArrivalRadius;
        }
        else
        {
            // Si la distancia es mayor, el agente se moverá con su velocidad máxima.
            speedToTarget = agent.MaxSpeed;
        }

        // TODO: No se si se hace así 
        // if (distance <= target.InteriorAngle)
        // {
        //     steer.angular = target.Orientation;
        // }

        Vector3 velocityToTarget = Vector3.Normalize(direction) * speedToTarget;
        steer.linear = (velocityToTarget - agent.Velocity) / timeToTarget;

        if (steer.linear.magnitude > agent.MaxSpeed)
        {
            steer.linear.Normalize();
            steer.linear *= agent.MaxSpeed;
        }
        
        return steer;
    }
}