using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Seek : SteeringBehaviour
{
    [SerializeField] protected Agent target;

    // TODO: Preguntar al profesor si esto es correcto
    // Por ahora esto funciona
    public virtual void NewTarget(Agent t)
    {
        target = t;
    }

    void Start()
    {
        nameSteering = "Seek Steering";
    }
    public Agent Target
    {
        get => target;
        set => target = value;
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        steer.angular = 0;
        // Donde queremos estar, es la posicion del nuevo soldado

        if (target == null)
        {
            steer.linear = -agent.Velocity;
            return steer;
        }

        // Posicion de origen
        Vector3 origen = agent.Position;

        // Posicion de destino.
        Vector3 destino = target.Position;

        // Se calcula el vector entre dos puntos.
        Vector3 direccion = destino - origen;

        // Comprobamos si hemos llegado al objetivo. Si es el caso, el agente se para.
        if (direccion.magnitude < target.InteriorRadius)
        {
            steer.linear += -agent.Velocity*4;
            return steer;
        }

        // Se normaliza el vector posicion del agente y se multiplica por la velocidad maxima del mismo.
        // El resultado obtenido sera un vector con la direccion y sentido con origen el agente y destino el target.
        // El modulo de dicho vector sera la velocidad indicada.
        direccion = Vector3.Normalize(direccion) * agent.MaxAcceleration;
        steer.linear = direccion;
        return steer;
    }
}