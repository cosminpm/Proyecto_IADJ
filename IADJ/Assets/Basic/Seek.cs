using UnityEngine;

public class Seek : SteeringBehaviour
{
    
    [SerializeField]
    private Vector3 _target;
    
    // Declara las variables que necesites para este SteeringBehaviour
    public Vector3 Target
    {
        get => _target;
        set =>_target = value;
    }

    // TODO: Preguntar al profesor si esto es correcto
    // Por ahora esto funciona
    public void NewTarget(Vector3 t)
    {
        _target = t;
    }
    
    void Start()
    {
        nameSteering = "Seek Steering";
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        
        // Donde queremos estar, es la posicion del nuevo soldado
        Vector3  origen= agent.Position;
        // Donde estamos ahora mismo
        Vector3 destino = _target;
        // Se calcula el vector entre dos puntos
        Vector3 direccion = destino - origen;
        // Se normaliza y se multiplica por la maxima aceleracion del agente
        direccion = Vector3.Normalize(direccion) * agent.MaxAcceleration;
        steer.linear = direccion;
        steer.angular = 0;
        
        return steer;
    }
}