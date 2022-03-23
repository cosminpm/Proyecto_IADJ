using UnityEngine;


public class Face : Align
{
    
    void Start()
    {
        nameSteering = "Face Steering";
    }

    public override Steering GetSteering(Agent agent)
    {

/*
        if (Target == null)
            return new Steering();

        Agent explicitTarget = Target;

        // Calculamos la dirección al objetivo.
        Vector3 direction = explicitTarget.Position - agent.Position;

        // Comprobamos que la longitud de la dirección no es cero. Si lo es, no hacemos nada.
        if (direction.magnitude == 0)
        {
            return new Steering();
        }

        // Juntamos los objetivos.
        Target = explicitTarget;
        Target.Orientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Delegamos el resultado a Align. 
        return base.GetSteering(agent);
        */

        return base.GetSteering(agent); 
    }
    
}
