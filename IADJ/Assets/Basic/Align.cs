using UnityEngine;

public class Align : SteeringBehaviour
{
    // Declaramos las variables que vamos a necesitar. 
    [SerializeField] protected Agent target;

    [SerializeField] private float timeToTarget = 1f;

    private float maxRotation;
    private float maxAngularAcceleration;

    private float targetRotation;

    

    // Obtenemos el target.
    public void NewTarget(Agent t)
    {
        target = t;
    }

    void Start()
    {
        nameSteering = "Align Steering";
        timeToTarget = 0.1f;
    }

    public override Steering GetSteering(Agent agent)
    {
        
        // Inicializamos las variables.
        maxAngularAcceleration = agent.MaxAngularAcceleartion;
        maxRotation = agent.MaxRotation;

        // Creamos el steering.
        Steering steer = new Steering();
        

        // Obtenemos la direccion del giro.
        float rotation = target.Orientation - agent.Orientation;

        Debug.Log("Agente hijoputas "+ agent.Orientation);

        
        // Mapeamos el resultado a un intervalo de (-180, 180) grados.
        rotation = mapToRange(rotation);
        Debug.Log("Agente hijoputas2 "+ agent.Orientation);
        float rotationSize = Mathf.Abs(rotation);
        Debug.Log("Agente hijoputas3 "+ agent.Orientation);
        // Si ya estamos en el objetivo, devolvemos un steering vacío.
        
        if (rotationSize < target.InteriorAngle)
        {
            // if (Mathf.Abs(agent.Rotation) < 0.1 )
                return steer;
        }
        Debug.Log("Agente hijoputas4 "+ agent.Orientation);
        
        // // Si estamos fuera del radio exterior, entonces usamos la máxima rotación.
        // if (rotationSize > target.ExteriorAngle){
        //     targetRotation = maxRotation;
        //     Debug.Log("Agente hijoputas5 "+ targetRotation);
        // }
        // // En otro caso calculamos una rotación escalada.
        // else{
        //     Debug.Log("maxRotation es "+maxRotation);
        //     Debug.Log("rotation size es "+rotationSize);
        //     Debug.Log("Exterior angle  es "+target.ExteriorAngle);

        //     targetRotation = maxRotation * rotationSize / target.ExteriorAngle;
        // }
        // La rotación objetivo final combina la velocidad y la dirección.
       
        targetRotation = maxRotation;
        targetRotation *= rotation / rotationSize;
        Debug.Log("Agente hijoputas66 "+ targetRotation);


        // La aceleración trata de de alcanzar la rotación objetivo.
        steer.angular = targetRotation - agent.Rotation; 
        Debug.Log("Agente hijoputas77 "+ steer.angular);
        Debug.Log("Agente hijoputas99 "+ timeToTarget);
        steer.angular /= timeToTarget;
        Debug.Log("Agente hijoputas88 "+ steer.angular);

        
        // Comprobamos si la aceleración es demasiado grande.
        float angularAcceleration = Mathf.Abs(steer.angular);


        // if (angularAcceleration > maxAngularAcceleration)
        // {
        //     steer.angular /= angularAcceleration;
        //     steer.angular *= maxAngularAcceleration;
        // }
        
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