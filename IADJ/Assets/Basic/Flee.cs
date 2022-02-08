using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{
    // Declaramos el objeto del que huir 
    [SerializeField]
    private Vector3 _target;
    // La distancia maxima que va a huir el personaje
    [SerializeField]
    private float disMaxFlee;
    
    
    // Declara las variables que necesites para este SteeringBehaviour
    public Vector3 Target
    {
        get => _target;
        set => _target = value;
    }

    public void NewTarget(Vector3 t)
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
        Vector3 destino = _target;

        // Se calcula el vector entre dos puntos
        Vector3 direccion = -(destino - origen);


        if (direccion.magnitude > disMaxFlee)
        {
            direccion = Vector3.zero;
        }
        
        // Se normaliza y se multiplica por la maxima aceleracion del agente
        direccion = Vector3.Normalize(direccion) * agent.MaxAcceleration;
        
        steer.linear = direccion;
        steer.angular = 0;

        return steer;
    }
}
