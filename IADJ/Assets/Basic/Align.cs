using UnityEngine;

public class Align : SteeringBehaviour
{
    // Declaramos las variables que vamos a necesitar. 
    [SerializeField] protected Agent target;

    [SerializeField] private float timeToTarget;

    private float targetRotation;

    protected Agent Target
    {
        get => target;
        set => target = value;
    }

    // Obtenemos el target.
    public void NewTarget(Agent t)
    {
        target = t;
    }

    void Start()
    {
        nameSteering = "Align Steering";
    }

    public override Steering GetSteering(Agent agent)
    {
        // Creamos el steering.
        Steering steer = new Steering();
        if (target == null)
        {
            steer.linear = Vector3.zero;
            return steer;
        }

        // Obtenemos la direccion de giro.
        float rotation = target.Orientation - agent.Orientation;

        // Mapeamos el resultado a un intervalo de (-180, 180) grados.
        rotation = mapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);
        
        if (rotationSize <= target.InteriorAngle)
        {
            if (Mathf.Abs(agent.Rotation) < 0.1 )
                return steer;
        }
        
        else if (rotationSize > target.ExteriorAngle)
            targetRotation = target.MaxRotation;
        else
            targetRotation = target.MaxRotation * rotationSize / target.ExteriorAngle;
        
        // Multiplicacion por +-1
        targetRotation *= rotation / rotationSize;
        steer.angular = targetRotation- agent.Rotation; 
        steer.angular /= timeToTarget;
        float angularAcceleration = Mathf.Abs(steer.angular);
        
        if (angularAcceleration > target.MaxAcceleration)
        {
            steer.angular /= angularAcceleration;
            steer.angular *= target.MaxAcceleration;
        }
        steer.angular = rotation;
        steer.linear = Vector3.zero;
        return steer;
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