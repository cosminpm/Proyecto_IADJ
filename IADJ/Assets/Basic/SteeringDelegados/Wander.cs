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
    private Agent _invisible;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = new GameObject("WanderTarget");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        Target = auxTarget.GetComponent<AgentInvisible>();

        _invisible = Target;
    }

    public override Steering GetSteering(Agent agent)
    {
        // Escribimos el movimiento que puede tener el target de wander. 
        // Mas concretamente, el target de wander solo podra moverse en un rango +-wanderRate.
        _wanderOrientation += Random.Range(-1f, 1f) * wanderRate;

        float targetOrientation = Mathf.Abs(_wanderOrientation) + agent.Orientation;
        _invisible.Orientation = targetOrientation;

        // Calculamos el centro del target de wander.
        Vector3 targetPosition = agent.Position + wanderOffset * agent.OrientationToVector();

        // Calculamos la posicion objetivo.
        targetPosition += wanderRadius * _invisible.OrientationToVector();
        _invisible.Position = targetPosition;

        // Delegamos a Face para que el personaje mire al target de wander.
        base.prota = _invisible;
        Steering steering = base.GetSteering(agent);

        // Sobreescribimos el movimiento lineal para que el personaje se dirija
        // a maxima aceleracion hacia la posicion indicada y con la orientacion que
        // acabamos de calcular.
        steering.angular *= Mathf.Rad2Deg;
        steering.linear = maxAcceleration * agent.OrientationToVector();

        return steering;
    }
}