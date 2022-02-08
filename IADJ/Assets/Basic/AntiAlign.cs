using UnityEngine;

public class AntiAlign : SteeringBehaviour
{
    
    [SerializeField]
    private Vector3 _target;

    public Vector3 Target
    {
        get => _target;
        set =>_target = value;
    }
    
    public void NewTarget(Vector3 t)
    {
        _target = t;
    }
    
    void Start()
    {
        nameSteering = "AntiAlign Steering";
    }
    
    public override Steering GetSteering(Agent agent)
    {
        return null;
    }
}