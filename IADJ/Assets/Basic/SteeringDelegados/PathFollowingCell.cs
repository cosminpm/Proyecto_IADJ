using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathFollowingCell : Seek
{
    // nodos del camino..
    [SerializeField] public PathCell path;

    // La posición actual del cámino
    [SerializeField] public int currentParam;

    // Nodo actual en el camino
    public int currentPos;

    // Posicion del target.
    protected int targetParam;

    protected int pathDir;

    void Start()
    {
        currentPos = 0;
        pathDir = 1;
        
        GameObject go = new GameObject("Invisible");
        Agent auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.ArrivalRadius = 0.5f;
        auxTarget.InteriorRadius = 0.5f;
        
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        Target = auxTarget;
    }

    public override void NewTarget(Agent t)
    {
        Target = t;
    }

    public bool EndPath(){
        return currentPos == path.Length()-1;
    }

    public override Steering GetSteering(Agent agent)
    {
        
        if (path == null)
        {
            return new Steering();
        }

        if (path.CondArrive(agent.Position, currentPos))
        {
            currentPos = currentPos + pathDir;
            if (currentPos > path.Length() || currentPos < 0)
            {
                currentPos = pathDir;
                //path.nodos = new List<Node>();
                path.DeletePath();
                return new Steering();
                //currentPos = path.Length() - 1;
            }
        }

        targetParam = currentPos;
        
        // La posicion del target a la que tiene que ir el agente
        Target.Position = path.GetPosition(targetParam);
        return base.GetSteering(agent);
    }
}