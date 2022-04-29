using UnityEngine;

public class Align : SteeringBehaviour
{
    // Declaramos las variables que vamos a necesitar . 
    [SerializeField] protected Agent target;

    [SerializeField] private float timeToTarget = 0.1f;

    private float maxRotation;
    private float maxAngularAcceleration;

    protected Agent Target
    {
        get => target;
        set => target = value;
    }

    int i = 0;us<dieebfiu8waebiutgbf<iawñbiut4

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

        // Inicializamos las variables.
        maxAngularAcceleration = agent.MaxAngularAcceleartion;
        maxRotation = agent.MaxRotation;

        // Creamos el steering.
        Steering steer = new Steering();

        if ( target == null)
            return steer;


        float targetRotation = 0f;
    
        // Obtenemos la direccion del giro.
        float rotation = target.Orientation - agent.Orientation;

        // Mapeamos el resultado a un intervalo de (-180, 180) grados.
        rotation = mapToRange(rotation);

        float rotationSize = Mathf.Abs(rotation);

        
         // Si ya estamos en el objetivo, devolvemos un steering vacío.
        if (rotationSize <= target.InteriorAngle){
            agent.Rotation = 0;
            return steer;
        }

        // Si estamos fuera del radio exterior, entonces usamos la máxima rotación.
        if (rotationSize > target.ExteriorAngle)
            targetRotation = maxRotation;

        // En otro caso calculamos una rotación escalada.
        else
            targetRotation = maxRotation * rotationSize / target.ExteriorAngle;

        // La rotación objetivo final combina la velocidad y la dirección.
        targetRotation *= rotation / rotationSize;

        // La aceleración trata de alcanzar la rotación objetivo.
        steer.angular = targetRotation - agent.Rotation;

        steer.angular /= timeToTarget;

        // Comprobamos si la aceleración es demasiado grande.

        float angularAcceleration = Mathf.Abs(steer.angular);

        if (angularAcceleration > maxAngularAcceleration)
        {
            steer.angular /= angularAcceleration;
            steer.angular *= maxAngularAcceleration;
        }

        steer.linear = Vector3.zero;

        return steer;
    }

    public float mapToRange(float rotation)
    {
        rotation %= 360;

        if (Mathf.Abs(rotation) >= 180)
        {
            if (rotation < 0.0f)
                rotation += 360;
            else
                rotation -= 360;
        }
        return rotation;
    }
}
