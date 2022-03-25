using UnityEngine;

public class Wander : Face
{
    // Distancia que separa el NPC del circulo de wander.
    [SerializeField]
    private int wanderOffset;

    // Radio del circulo de wander.
    [SerializeField]
    private int wanderRadius;

    // Cambio maximo que puede tener el target de wander.
    [SerializeField]
    private int wanderRate;

    // Aceleracion maxima del NPC.
    [SerializeField]
    private int maxAcceleration;

    // Orientacion del target de wander.
    private int wanderOrientation;

    // Target de wander.
    private AgentInvisible invisible;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ob = new GameObject();

        ob = Instantiate(ob, Vector3.zero, Quaternion.identity);
        ob.AddComponent<AgentInvisible>();
        ob.GetComponent<AgentInvisible>().DrawGizmos = true;
        ob.name = "Target Wander";
        invisible = ob.GetComponent<AgentInvisible>();

    }

    public override Steering GetSteering(Agent agent)
    {
        // Declaramos la variable aleatoria.
        var rand = new System.Random();

        // Escribimos el movimiento que puede tener el target de wander. 
        // Mas concretamente, el target de wander solo podra moverse en un rango +-wanderRate.
        wanderOrientation += rand.Next(-wanderRate, wanderRate);


        float targetOrientation = wanderOrientation + agent.Orientation;
        invisible.Orientation = targetOrientation;
        Debug.Log("ORIENTACION_INVISIBLE: " + wanderOrientation);

        // Calculamos el centro del target de wander.
        Vector3 targetPosition = agent.Position + wanderOffset * agent.OrientationToVector();

        // Calculamos la posicion objetivo.
        targetPosition += wanderRadius * invisible.OrientationToVector();

        // Delegamos a Face para que el personaje mire al target de wander.
        invisible.Position = targetPosition;
        NewTarget(invisible);
        Steering steering = base.GetSteering(agent);

        // Sobreescribimos el movimiento lineal para que el personaje se dirija
        // a maxima aceleracion hacia la posicion indicada y con la orientacion que
        // acabamos de calcular.
        steering.linear = maxAcceleration * agent.OrientationToVector();

        return steering;
        
    }

}