using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHp : State
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
    }

    public override void CheckState(NPC npc)
    {
        // TODO: Aqu� hay que comprobar si ha llegado a una zona de
        // curaci�n. Si ese es el caso habr� que pasar a un estado Healing.
        //
        // Tambi�n hay que comprobar si el NPC est� muerto, ya que alguien
        // puede matarlo por el camino.
        if (IsDead(npc))
            return;
    }

    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }
}
