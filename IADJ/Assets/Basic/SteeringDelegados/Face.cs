using UnityEngine;


public class Face : Align
{
    private Agent Target
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
        nameSteering = "Face Steering";
    }

    public override Steering GetSteering(Agent agent)
    {

        if (target == null)
            return new Steering();

        Agent explicitTarget = this.target;

        // Calculamos la dirección al objetivo.
        Vector3 direction = explicitTarget.Position - agent.Position;

        // Comprobamos que la longitud de la dirección no es cero. Si lo es, no hacemos nada.
        if (direction.magnitude == 0)
        {
            return new Steering();
        }

        // Juntamos los objetivos.
        base.target = explicitTarget;
        base.target.Orientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Delegamos el resultado a Align. 
        return base.GetSteering(agent);
    }
}
