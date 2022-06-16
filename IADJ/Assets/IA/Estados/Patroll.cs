using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Patroll : State
{
    private float THRESHOLD = 4f;
    private bool onWay = false;
    private Vector3 patrollGoal;

    void Awake()
    {
        stateImage.enabled = true;
    }

    public override void EntryAction(NPC npc)
    {
        onWay = false;
        movement = false;
    }

    public override void ExitAction(NPC npc)
    {
        npc.pathFinding.ClearPath();
        _targetNPC = null;
    }


    public override void Action(NPC npc)
    {
        // Calcular el waypoint mas cercano
        if (!onWay)
        {
            patrollGoal = npc.GameManager.waypointManager.GetClosestPatrollWaypoint(npc);
            onWay = true;
        }

        else
        {
            Vector3 direction = patrollGoal - npc.GetUnitPosition();
            float goalDistance = direction.magnitude;

            if (!movement)
            {
                npc.pathFinding.CalculatePath(patrollGoal);
                movement = true;
            }

            // Sabemos que estamos en el waypoint si la distancia que nos
            // separa de el es menor a una cierta distancia. 
            else if (goalDistance < THRESHOLD)
            {
                patrollGoal = npc.GameManager.waypointManager.GetNextPatrollWaypoint(npc);
                npc.pathFinding.CalculatePath(patrollGoal);
            }
        }
    }


    public override void CheckState(NPC npc)
    {

        // Si estoy muerto, cambio al estado a muerto
        if (npc.stateManager.IsDead())
            return;

        if (npc.stateManager.TotalWar())
            return;

        if (npc.stateManager.AllieHealthReached())
            return;
    }

    public override void Execute(NPC npc)
    {
        Action(npc);
        CheckState(npc);
    }
}