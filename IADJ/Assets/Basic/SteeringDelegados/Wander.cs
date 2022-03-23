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

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    public override Steering GetSteering(Agent agent)
    {
        // Declaramos la variable aleatoria.
        var rand = new System.Random();

        // Escribimos el movimiento que puede tener el target de wander. 
        // Mas concretamente, el target de wander solo podra moverse en un rango +-wonderRate.
        wanderOrientation += rand.Next(-1, 1) * wanderRate;

        target.Orientation = wanderOrientation + agent.Orientation;

        // Calculamos el centro del target de wander.
        target.Position = agent.Position + wanderOffset * agent.OrientationToVector();

        // Calculamos la posicion objetivo.
        target.Position += wanderRadius * target.OrientationToVector();

        // Delegamos a Face para que el personaje mire al target de wander.
        Steering steering = base.GetSteering(agent);

        // Sobreescribimos el movimiento lineal para que el personaje se dirija
        // a maxima aceleracion hacia la posicion indicada y con la orientacion que
        // acabamos de calcular.
        steering.linear = maxAcceleration * agent.OrientationToVector();

        return steering;
        
    }

}
