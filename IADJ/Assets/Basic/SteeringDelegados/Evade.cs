using UnityEngine;

public class Evade : Flee
{
    [SerializeField] public float maxPrediccion;

    private GameObject prediccionGO;
    private Agent targetAux;
    
  

    void Start(){

        targetAux = Target;
        prediccionGO = new GameObject("auxEvade");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        invisible.GetComponent<AgentInvisible>().DrawGizmos = true;
        invisible.ArrivalRadius = 3f;
        invisible.InteriorRadius = 3f;

        Target = invisible;
    }

    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();
        
        if (Target == null)
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

        
        return base.GetSteering(agent);
    }
}