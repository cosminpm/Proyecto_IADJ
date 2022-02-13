using System.Collections.Generic;
using UnityEngine;

public class AgentNPC : Agent
{
    // Este será el steering final que se aplique al personaje.
    public Steering steer;
    public List<SteeringBehaviour> listSteerings;

    public override void Awake()
    {
        steer = new Steering();

        // Construye una lista con todos las componenen del tipo SteeringBehaviour.
        // La llamaremos listSteerings
        // Usa GetComponents<>()
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
        Position += Velocity  * Time.deltaTime;
        Rotation += steer.angular * Time.deltaTime;
        
        Orientation = mapToRange(Rotation) ;
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, Orientation);
    }
    
    public virtual void LateUpdate()
    {
        // Reseteamos el steering final.
        steer = new Steering();
        
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
