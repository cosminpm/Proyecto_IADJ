using UnityEngine;

public class Align : SteeringBehaviour
{
    
    [SerializeField]
    private Agent target;
    
    // Declara las variables que necesites para este SteeringBehaviour
    public Agent Target
    {
        get => target;
        set =>target = value;
    }

    // TODO: Preguntar al profesor si esto es correcto
    // Por ahora esto funciona
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

        Steering steer = new Steering();
        steer.linear = Vector3.zero;
        
        if (target == null)
        {
            steer.linear = Vector3.zero;
            return steer;
        }
        Debug.Log("aaa");

        float rotation = target.Orientation - agent.Orientation;
        rotation = mapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < target.InteriorRadius)
        {
            steer.angular = -target.Rotation;
            return steer;
        }
        
        float vRotation = target.MaxRotation * rotationSize / target.ExteriorAngle;
        steer.angular = vRotation;
        
        return steer;
    }

    private float mapToRange(float rotation)
    {
        rotation %=  Mathf.PI * 2;
        
        if (Mathf.Abs(rotation) >  Mathf.PI) {
            if (rotation < 0.0f)
                rotation += Mathf.PI * 2;
            else
                rotation -=  Mathf.PI * 2;
        }
        return rotation;
    }
}