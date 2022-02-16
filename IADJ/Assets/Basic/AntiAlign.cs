using UnityEngine;

public class AntiAlign : SteeringBehaviour
{
    
    [SerializeField]
    private Vector3 target;
    
    public void NewTarget(Vector3 t)
    {
        target = t;
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