using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : State
{
    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = true;
        _targetNPC = null;
        Debug.Log("Me queda poca vida :(");
    }

    public override void ExitAction(NPC npc)
    {
    }

    // TODO: En este estado, el NPC deber� buscar la zona de curaci�n m�s 
    // cercana y huir hacia all�.
    public override void Action(NPC npc, NPC obj)
    {
        if (npc.Unit.CurrentHealthPoints < npc.Unit.HealthPointsMax)
            npc.Unit.CurrentHealthPoints += 10;
    }

    public override void CheckState(NPC npc)
    {
        if (IsDead(npc) || HealingFinished(npc))
            return;
    }

    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }
}
