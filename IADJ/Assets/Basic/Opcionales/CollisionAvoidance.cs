using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehaviour {

    // Lista de targets a evitar
    [SerializeField] List<Agent> targets;
    
    // Radio de colisión del personaje
    [SerializeField] float radius;

    void Start(){

        // Obtenemos todos los agents NPC.
        GameObject[] gos = GameObject.FindGameObjectsWithTag("NPC");

        // Añadimos a la lista de targets los agentes.
        foreach( var go in gos)
        {
            targets.Add(go.GetComponent<Agent>());
        }

        // TODO: que pasa con el agente NPC que hace este steering?

    }

    public virtual void NewTarget(Agent t)
    {
    
    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steering = new Steering();

        // Tiempo de la primera colisión
        float shortestTime = Mathf.Infinity;

        // Variables que necesitamos para calcular y evitar la colisión
        Agent firstTarget = null;
        float firstMinSeparation = 0.0f;
        float firstDistance = 0.0f;
        Vector3 firstRelativePos = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;

        float timeToCollision = 0.0f;


        Vector3 relativePos = Vector3.zero;
        Vector3 relativeVel = Vector3.zero;
        float relativeSpeed = 0;
        float dot = 0;
        float distance = 0;
        float minSeparation = 0;

        

        foreach (var target in targets)
        {
            // Calculamos el tiempo de colisión
            relativePos = target.Position - agent.Position;
            relativeVel = target.Velocity - agent.Velocity;
            relativeSpeed = relativeVel.magnitude;
            dot = Vector3.Dot(relativePos,relativeVel);
            timeToCollision = -(dot/(relativeSpeed * relativeSpeed));

            distance = relativePos.magnitude;

            // Distancia más pequeña
            minSeparation = distance-relativeSpeed*timeToCollision;

             // Comprobamos si el agente no va a colisonar
            if (minSeparation > 2*radius) {
                 continue;

            }
            
            // Comprobamos si el tiempo de colisión es inferior al tiempo de las demás colisiones
            if (timeToCollision > 0 && timeToCollision < shortestTime){

                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }

        // Cálculo del steering

        // Comprobamos si no hay ninguna colision
        if ( firstTarget == null){
            Debug.Log("No hay ninguna colisión ");
            return steering;
        }
      
        // Si vamos a chocar o estamos colisionando
        if ( firstMinSeparation <= 0 || firstDistance < 2*radius){
            Debug.Log("Estamos cHOCANDO!! ");
            relativePos = firstTarget.Position;
        } else {
            Debug.Log("Vamos a colisionar maamahuevo ");
            relativePos = firstRelativePos + firstRelativeVel * shortestTime;
        }

        relativePos.Normalize();
        steering.linear =  relativePos * agent.MaxAcceleration;
        return steering;
    }
}