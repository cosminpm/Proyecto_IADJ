using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbitroPonderado : ArbitroManager {

    public void Start(){
        Inicializar();
    }

    public override Steering GetSteering(Agent agent)
    {
      Steering steering = new Steering();

        // Acumulamos todos los movimientos de la lista 
        foreach ( var steer in listaBehaviours){
            if ( steer.enabled){
                Steering aux = steer.GetSteering(agent);
                steering.linear += steer.weight * aux.linear;
                steering.angular += steer.weight * aux.angular;
            }
        }

        float auxLinear = Mathf.Min(steering.linear.magnitude, agent.MaxSpeed);
        steering.linear = steering.linear * auxLinear;
        steering.angular = Mathf.Min(steering.angular, agent.MaxRotation);

        return steering;

    }
}