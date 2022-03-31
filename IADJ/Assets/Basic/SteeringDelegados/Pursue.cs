using UnityEngine;

public class Pursue : Seek
{
    [SerializeField] public float maxPrediccion;

    private Agent targetAux;
    private GameObject prediccionGO;
    public override void NewTarget(Agent t)
    {
        //base.NewTarget(t);
        targetAux = Target;

        GameObject prediccionGO = new GameObject("AuxPursue");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = true;
        Target = invisible;
    }

    void Start(){
        group = 2;


        targetAux = Target;

        prediccionGO = new GameObject("AuxPursue");
        AgentInvisible arg = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = true;
        Target = arg;
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        
        if (target == null)
        {
            steer.linear = Vector3.zero;
            return steer;
        }
        
        // Se calcula la distancia al targetPerseguido
        Vector3 direccion = targetAux.Position - agent.Position;
        float distancia = direccion.magnitude;
        
        // Se calcula la velocidad del agente
        float velocidad = agent.Velocity.magnitude;
        
        // Tiempo de predicción
        float prediccion;
        
        // Si la velocidad del NPC es mayor que la velocidad a la que tendría que ir el NPC para alcanzar el targetPerseguido en un
        // tiempo maxPrediccion, 
        if (velocidad > (distancia / maxPrediccion))
        {
            prediccion = distancia / velocidad;
        }
        else
        {
            prediccion = maxPrediccion;
        }
        Target.Position = targetAux.Position;
        Target.Position += targetAux.Velocity * prediccion;
        prediccionGO.transform.position = Target.Position;
        
        return base.GetSteering(agent);
    }
}