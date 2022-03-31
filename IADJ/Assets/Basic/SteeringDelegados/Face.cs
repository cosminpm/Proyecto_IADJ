using UnityEngine;


public class Face : Align
{
    private Agent targetAux;

    void Start()
    {
        targetAux = Target;
        GameObject prediccionGO = new GameObject("AuxFace");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = true;
        Target = invisible;

        group = 2;
    }


    public override Steering GetSteering(Agent agent)
    {
        if ( targetAux == null && Target == null)
        {
            Debug.Log("JAJAJAJA");
            return new Steering();
        }

        targetAux = Target;

        Debug.Log(agent.name);

        // Calculamos la dirección al objetivo.
        Vector3 direction = targetAux.Position - agent.Position;

        // Comprobamos que la longitud de la dirección no es cero. Si lo es, no hacemos nada.
        if (direction.magnitude == 0)
        {
            return new Steering();
        }

        Target.Orientation =  Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Debug.Log(" Target Orientation "+Target.Orientation);
        // Delegamos el resultado a Align. 
        return base.GetSteering(agent);
    }
}