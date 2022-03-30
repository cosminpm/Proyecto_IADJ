using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbitroPonderado : ArbitroManager
{
    // Otra duda, cuando creo la lista de steering, no me deja poner los scripts.. solo objetos..

    // Todo: No hacen faltan targets..
    public virtual void NewTarget(Agent t)
    {
        /*
        foreach ( var steer in listaBehaviours){
    
            if (steer is Seek )
            {
                (Seek)steer.Target = t;
            }
        }
        */
    }

    public void Start()
    {
        Inicializar();
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steering = new Steering();
   
        // Acumulamos todos los movimientos de la lista 
        foreach (var steer in listaBehaviours)
        {
            if (steer.enabled)
            {
                Steering aux = steer.GetSteering(agent);
                steering.linear += steer.weight * aux.linear;
                steering.angular += steer.weight * aux.angular;
            }
        }

        // DUDA. SE UTILIZA LA MaxAcceleration?
        
        float auxLinear = Mathf.Min(steering.linear.magnitude, agent.MaxSpeed);
        steering.linear = steering.linear * auxLinear;
        steering.angular = Mathf.Min(steering.angular, agent.MaxRotation);
        
        return steering;
    }
}