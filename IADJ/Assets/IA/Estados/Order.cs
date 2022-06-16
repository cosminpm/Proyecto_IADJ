using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Order : State
{
     void Awake(){
        stateImage.enabled = false;
    }

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

    }

    public override void CheckState(NPC npc)
    {
  
        if (npc.stateManager.IsDead())
            return;

        if ( npc.pathFinding.IsEndPath())
        {
            npc.stateManager.ChangeState(npc.stateManager.stateCapture);
            return;
        }

        
    }

    public override void Execute(NPC npc)
    {
        Action(npc);
        CheckState(npc);
    }
}