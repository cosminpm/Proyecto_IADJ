using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : State
{
    void Awake()
    {
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = false;
    }


    public override void ExitAction(NPC npc)
    {
        npc.pathFinding.ClearPath();
    }

    public override void Action(NPC npc)
    {

        if (!movement)
        {
            npc.pathFinding.CalculatePath(_targetNPC.GetUnitPosition());
            movement = true;
        }

    }


    public override void CheckState(NPC npc)
    {
        // Si estoy muerto, cambio al estado a muerto
        if (npc.stateManager.IsDead())
            return;

        if (npc.stateManager.TotalWar())
            return;

        if (npc.stateManager.IsLowHP())
            return;

        // Si hay alg�n al que atacar, cambio de estado a MeleeAttack
        if ( npc.pathFinding.IsEndPath() && npc.stateManager.EnemyFound())
        {
            return;
        }


    }


    public override void Execute(NPC npc)
    {
        Action(npc);
        CheckState(npc);
    }



}
