using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : State
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

    public override void Action(NPC npc, NPC obj)
    {

        if (!movement)
        {
            npc.pathFinding.CalculatePath(npc.GameManager.waypointManager.GetBasePosition(npc));
            movement = true;
        } 
        
    }


    public override void CheckState(NPC npc)
    {
         // Si estoy muerto, cambio al estado a muerto
        if  (npc.stateManager.IsDead())
             return;

        if (npc.stateManager.IsLowHP(npc))
            return;

        // // Si hay enemigos en nuestra base, vamos hacia allá.
        // if (npc.stateManager.EnemiesInBase(npc))
        //     return;

        // Si hay algún al que atacar, cambio de estado a MeleeAttack
        if (EnemyFound(npc)){
            return;
        }


    }


    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }



}
