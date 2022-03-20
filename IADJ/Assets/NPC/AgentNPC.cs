using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public Steering steer;


    // Lista de steering que tiene el personaje: TODO A LO MEJOR NO HACE FALTA..
    public List<SteeringBehaviour> listSteerings;


    // Arbitro que tiene el personaje
    public ArbitroManager arbitro;

    public override void Awake()
    {
        // Inicializamos el steering del personaje
        steer = new Steering();

        // Obtenemos el arbitro
        arbitro = GetComponent<ArbitroManager>();

        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        GetComponents<SteeringBehaviour>(listSteerings);
    }

    // Use this for initialization
    void Start()
    {
        Velocity = Vector3.zero;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        ApplySteering();
    }

    private void ApplySteering()
    {
        Velocity += steer.linear * Time.deltaTime;
        Position += Velocity * Time.deltaTime;
        Rotation += steer.angular * Time.deltaTime;

        Orientation = Orientation + steer.angular * Time.deltaTime;
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, Orientation);
    }

    public virtual void LateUpdate()
    {
        // Reseteamos el steering final.
        //   steer = new Steering();

        // Si hay arbitro.
        if (arbitro != null)
        {
            steer = arbitro.GetSteering(this);
            return;
        }

        // Recorremos cada steering
        foreach (SteeringBehaviour behavior in listSteerings)
            GetSteering(behavior);
    }

    private void GetSteering(SteeringBehaviour behavior)
    {
        // Calcula el steeringbehaviour
        Steering kinematic = behavior.GetSteering(this);

        // Usar kinematic con el árbitro desesado para combinar todos los SteeringBehaviour.
        // Llamaremos kinematicFinal a la aceleraciones finales.
        Steering kinematicFinal = kinematic; // Si solo hay un SteeringBehaviour

        // El resultado final se guarda para ser aplicado en el siguiente frame.
        steer = kinematicFinal;
    }

    private float mapToRange(float rotation)
    {
        rotation %= 360;

        if (Mathf.Abs(rotation) > 180)
        {
            if (rotation < 0.0f)
                rotation += 360;
            else
                rotation -= 360;
        }

        return rotation;
    }
}