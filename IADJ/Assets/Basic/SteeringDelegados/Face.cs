using UnityEngine;


public class Face : Align
{
    private Agent targetAux;

    void Awake()
    {
        targetAux = Target;
        GameObject prediccionGO = new GameObject("AuxFace");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = false;
        invisible.InteriorAngle = targetAux.InteriorAngle;
        invisible.ExteriorAngle = targetAux.ExteriorAngle;
        Target = invisible;
    }


    public override Steering GetSteering(Agent agent)
    {

        if ( targetAux == null)
        {
           GameObject go = new GameObject("FaceTarget");
           Agent auxTarget = go.AddComponent<AgentInvisible>();
           auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
           targetAux = auxTarget; 
        }

        if ( targetAux == null && Target == null)
        {
            return new Steering();
        }
         
        // Calculamos la dirección al objetivo.
        Vector3 direction = targetAux.Position - agent.Position;

        // Comprobamos que la longitud de la dirección no es cero. Si lo es, no hacemos nada.
        if (direction.magnitude == 0)
        {
            return new Steering();
        }

        Target.Orientation =  Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Delegamos el resultado a Align. 
        return base.GetSteering(agent);
    }

    
}