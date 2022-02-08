using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Seek : SteeringBehaviour
{
    [SerializeField] private Vector3 target;

    [SerializeField] private float threshHold = 0.5f;

    // Declara las variables que necesites para este SteeringBehaviour
    public Vector3 Target
    {
        get => target;
        set => target = value;
    }

    // TODO: Preguntar al profesor si esto es correcto
    // Por ahora esto funciona
    public void NewTarget(Vector3 t)
    {
        target = t;
    }

    void Start()
    {
        nameSteering = "Seek Steering";
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        steer.angular = 0;
        // Donde queremos estar, es la posicion del nuevo soldado

        if (target == Vector3.zero)
        {
            steer.linear = -agent.Velocity;
            return steer;
        }
        
        Vector3 origen = agent.Position;
        // Donde estamos ahora mismo
        Vector3 destino = target;
        // Se calcula el vector entre dos puntos
        Vector3 direccion = destino - origen;
        // Se normaliza y se multiplica por la maxima aceleracion del agente
        
        
        if (direccion.magnitude < threshHold)
        {
            direccion = -agent.Velocity;
            steer.linear = direccion;
            return steer;
        }

        direccion = Vector3.Normalize(direccion) * agent.MaxSpeed;
        steer.linear = direccion;
        return steer;
    }
}