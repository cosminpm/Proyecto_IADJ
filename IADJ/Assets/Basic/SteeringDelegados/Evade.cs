using UnityEngine;

public class Evade : Flee
{
    private Agent targetAux;
    [SerializeField]
    public float maxPrediccion;

    void Start() {

        if ( GameObject.Find("AgenteInvisible"))
        {
            targetAux = this.Target;
            GameObject goPursue = GameObject.Find("AgenteInvisible");
            this.Target = goPursue.AddComponent<AgentInvisible>();
            goPursue.GetComponent<AgentInvisible>().DrawGizmos = true;
        }
    
    }
    
    public override Steering GetSteering(Agent agent)
    {
        // Se calcula la distancia al targetPerseguido
        Vector3 direccion = targetAux.Position - agent.Position;
        float distancia = direccion.magnitude;

        // Se calcula la velocidad del agente
        float velocidad = agent.Velocity.magnitude;  

        // Tiempo de predicción
        float prediccion;

        // Si la velocidad del NPC es mayor que la velocidad a la que tendría que ir el NPC para alcanzar el targetPerseguido en un
        // tiempo maxPrediccion, 
        if ( velocidad > (distancia/maxPrediccion) )
        {
            prediccion = distancia/velocidad;
        }
        else
        {
            prediccion = maxPrediccion;
        }
        
    	// Put the targetPerseguido together
        this.Target.Position = targetAux.Position;
        this.Target.Position += targetAux.Velocity * prediccion;
        
        return base.GetSteering(agent);
    }
}
