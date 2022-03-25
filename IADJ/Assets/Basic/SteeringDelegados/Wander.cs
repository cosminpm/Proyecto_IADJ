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

    // Aceleracion maxima del NPC.
    [SerializeField] private float maxAcceleration;

    // Orientacion del target de wander.
    private float _wanderOrientation;

    // Target de wander.
    private AgentInvisible _invisible;


    public bool drawWander;


    // Start is called before the first frame update
    void Start()
    {
        GameObject ob = new GameObject();
        ob = Instantiate(ob, Vector3.zero, Quaternion.identity);
        ob.AddComponent<AgentInvisible>();
        ob.GetComponent<AgentInvisible>().DrawGizmos = true;
        ob.name = "Target Wander";
        _invisible = ob.GetComponent<AgentInvisible>();
    }

    public override Steering GetSteering(Agent agent)
    {
        // Escribimos el movimiento que puede tener el target de wander. 
        // Mas concretamente, el target de wander solo podra moverse en un rango +-wanderRate.

        _wanderOrientation += Random.Range(-1f, 1f) * wanderRate;
        
        float targetOrientation = _wanderOrientation + agent.Orientation;

        _invisible.Orientation = targetOrientation;
        // Calculamos el centro del target de wander.
        Vector3 targetPosition = agent.Position + wanderOffset * agent.OrientationToVector();

        // Calculamos la posicion objetivo.
        targetPosition += wanderRadius * _invisible.OrientationToVector();

        
        _invisible.Position = targetPosition;
        NewTarget(_invisible);

        // Delegamos a Face para que el personaje mire al target de wander.
        Steering steering = base.GetSteering(agent);

        // Sobreescribimos el movimiento lineal para que el personaje se dirija
        // a maxima aceleracion hacia la posicion indicada y con la orientacion que
        // acabamos de calcular.
        steering.linear = maxAcceleration * agent.OrientationToVector();
        Debug.Log(agent.OrientationToVector());
        return steering;
    }


    private void OnDrawGizmos()
    {
        if (drawWander && _invisible != null)
        {
            var position = transform.position;
            Vector3 centroCirculo = new Vector3(position.x + wanderOffset, position.y, position.z + wanderOffset);
            Handles.DrawLine(position, centroCirculo);
            Handles.DrawWireDisc(centroCirculo, Vector3.up, wanderRadius, 3);

            Vector3 circuloSmall = new Vector3(centroCirculo.x + wanderRadius, centroCirculo.y,
                centroCirculo.z + wanderRadius);
            Handles.DrawWireDisc(circuloSmall, Vector3.up, wanderRate, 3);
        }
    }
}