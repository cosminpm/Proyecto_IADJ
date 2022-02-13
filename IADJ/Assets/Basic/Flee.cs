using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{
    // Declaramos el objeto del que huir 
    [SerializeField]
    private Agent _target;
    // La distancia maxima que va a huir el personaje
    [SerializeField]
    private float disMaxFlee;
    
    
    // Declara las variables que necesites para este SteeringBehaviour
    public Agent Target
    {
        get => _target;
        set => _target = value;
    }

    public void NewTarget(Agent t)
    {
        _target = t;
    }

    // Start is called before the first frame update
    void Start()
    {
        nameSteering = "Flee Steering";
    }

    public override Steering GetSteering(Agent agent)
    {
        // Construimos el nuevo steering
        Steering steer = new Steering();

        // Calculamos la posicion contraria a la del agente (la posicion objetivo)
        Vector3 origen = agent.Position;

        // Calculamos la posicion actual
        Vector3 destino = _target.Position;

        // Se calcula el vector entre dos puntos
        Vector3 direccion = new Vector3(-(destino.x - origen.x),0, -(destino.z - origen.z));
        
        if (direccion.magnitude > disMaxFlee)
        {
            direccion = -agent.Velocity;
            steer.linear = direccion;
            return steer;
        }
        
        // Se normaliza y se multiplica por la maxima aceleracion del agente
        Vector3 vVelocidad = Vector3.Normalize(direccion) * agent.MaxSpeed;

        steer.linear = direccion;
        steer.angular = 0;

        return steer;
    }
}
