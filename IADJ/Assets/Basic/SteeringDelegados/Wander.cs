using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wander : Face
{
    // Distancia que separa el NPC del circulo de wander.
    [SerializeField] private float wanderOffset;

    // Radio del circulo de wander.
    [SerializeField] private float wanderRadius;

    // Cambio maximo que puede tener el target de wander.
    [SerializeField] private float wanderRate;


    // Orientacion del target de wander.
    private float _wanderOrientation;


    [SerializeField] protected bool _drawGizmos;

    private AgentInvisible agentInvisible;

    private float auxOrientation = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = new GameObject("WanderTarget");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        Target = auxTarget;


        GameObject faceGO = new GameObject("AuxFace");
        AgentInvisible faceInvisible = faceGO.AddComponent<AgentInvisible>();
        faceGO.GetComponent<AgentInvisible>().DrawGizmos = true;

        agentInvisible = faceInvisible;
    }


    public override Steering GetSteering(Agent agent)
    {

         // Para gizmos
        auxOrientation = agent.Orientation;

        // Escribimos el movimiento que puede tener el target de wander. 
        // Mas concretamente, el target de wander solo podra moverse en un rango +-wanderRate.
        _wanderOrientation += Random.Range(-1f, 1f) * wanderRate;

        float targetOrientation = _wanderOrientation + agent.Orientation;

        // Calculamos el centro del target de wander.
        Vector3 targetPosition = agent.Position + wanderOffset * OrientationToVector(agent.Orientation);

        
        // Calculamos la posicion objetivo.
        targetPosition += wanderRadius * OrientationToVector(targetOrientation);

   
        // Delegamos a Face para que el personaje mire al target de wander.
        agentInvisible.Position = targetPosition;
        Target = agentInvisible;


        // Creamos el target que vamos a delegar a face **
        Steering steer = base.GetSteering(agent);
        // Sobreescribimos el movimiento lineal para que el personaje se dirija
        // a maxima aceleracion hacia la posicion indicada y con la orientacion que
        // acabamos de calcular.
        steer.linear = agent.MaxAcceleration * OrientationToVector(agent.Orientation);

        return steer;
    }

    private Vector3 OrientationToVector(float _orientation)
    {

        Vector3 aux = new Vector3(Mathf.Sin(_orientation * Mathf.Deg2Rad), 0, Mathf.Cos(_orientation * Mathf.Deg2Rad));
        return aux.normalized;
    }


    

    public void OnDrawGizmos()
    {
        if (_drawGizmos) {
            
            // Wander Radius
            Handles.color = Color.blue;
            Vector3 aux = transform.position;
            aux += OrientationToVector(auxOrientation) * wanderOffset;
            Handles.DrawWireDisc(aux, Vector3.up, wanderRadius, 3);

             // Wander offset
            Handles.color = Color.magenta;
            Handles.DrawLine(transform.position,aux,1);

            // Direction to Target
            Handles.color = Color.cyan;
            if ( Target != null)
                Handles.DrawLine(transform.position, Target.Position, 1);

        }
    }

}