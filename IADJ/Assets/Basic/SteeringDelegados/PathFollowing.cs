using UnityEngine;

public class PathFollowing : Seek
{
    // nodos del camino..
    [SerializeField] public Path path;

    // La posición actual del cámino
    [SerializeField] public int currentParam;

    // Nodo actual en el camino
    protected int currentPos;

    // Posicion del target.
    protected int targetParam;
  
    protected int pathDir;

    void Start(){
        currentPos = 0;
        pathDir = 1;
    }

    public override void NewTarget(Agent t)
    {
       Target = t;
    }

    public override Steering GetSteering(Agent agent)
    {


        if (path.condArrive(agent.Position, currentPos)) {

            currentPos = currentPos + pathDir;
            if (currentPos >= path.length() || currentPos < 0) {
                pathDir *= -1;
                currentPos += pathDir;
            }
            targetParam = currentPos;
        } else {
            targetParam = currentPos;
        }

        
      // La posicion del target a la que tiene que ir el agente
        Target.Position = path.GetPosition(targetParam);
        return base.GetSteering(agent);
    }
   
}