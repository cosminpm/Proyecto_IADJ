using UnityEngine;


public class Face : Align
{
    Agent explicitTarget;
    protected Agent prota { get; set; }

    void Start()
    {
        nameSteering = "Face Steering";
        prota = Target;
    }

    public override Steering GetSteering(Agent agent)
    {
        if (explicitTarget == null)
        {
            GameObject go = new GameObject("FaceTarget");
            Agent auxTarget = go.AddComponent<AgentInvisible>();
            auxTarget.GetComponent<AgentInvisible>().DrawGizmos = false;
            explicitTarget = auxTarget;
        }

        if (prota == null && GameObject.Find("AgenteInvisible"))
            prota = GameObject.Find("AgenteInvisible").GetComponent<Agent>();

        if (prota == null)
            return new Steering();

        // Calculamos la dirección al objetivo.
        Vector3 direction = prota.Position - agent.Position;

        // Comprobamos que la longitud de la dirección no es cero. Si lo es, no hacemos nada.
        if (direction.magnitude == 0)
        {
            return new Steering();
        }

        // Juntamos los objetivos.
        Target = explicitTarget;

        float targetOrientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Target.Orientation = targetOrientation;

        // Delegamos el resultado a Align. 
        if (direction.magnitude < 1)
            agent.Rotation = 0;

        return base.GetSteering(agent);
    }
}