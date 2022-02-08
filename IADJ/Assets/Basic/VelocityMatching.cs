using UnityEngine;

public class VelocityMatching : SteeringBehaviour
{
    /*
     * Velocity Matching consiste en igualar la velocidad del usuario actual con la velocidad del agente seleccionado
     */
    [SerializeField]
    private float timeToTarget = 0.1f;
    public void NewTarget(Agent a)
    {
        _target = a;
    }
    
    
    [SerializeField]
    private Agent _target;
    
    
    public Agent Target
    {
        get => _target;
        set =>_target = value;
    }
    
    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        
        steer.angular = 0;
        if (_target == null)
        {
            steer.linear = Vector3.zero;
            return steer;
        }
        
        steer.linear = (_target.Velocity - agent.Velocity) / timeToTarget;
        //Debug.Log(_target.name);
        if (steer.linear.magnitude > agent.MaxAcceleration)
        {
            steer.linear.Normalize();
            steer.linear *= agent.MaxAcceleration;
        }
        
        //Debug.Log(steer.linear);
        return steer;
    }
}