using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : State
{
    // Variable para comprobar, en frames, el tiempo de respown.
    // 600 frames = 10s
    private int _timeRespown = 1800;
    public override void EntryAction(NPC npc)
    {
        // El NPC deber�a hacer respown en su base. 
        npc.Unit.UnitAgent.Position = new Vector3(-115, 0, 100);
        Debug.Log("Me mor� T.T");
    }

    public override void ExitAction(NPC npc)
    {
        // movement = false;
        // _targetNPC = null;
    }

    // Cambiamos la posici�n del NPC y hacemos que se cure por completo.
    public override void Action(NPC npc, NPC obj)
    {
        npc.Unit.CurrentHealthPoints = npc.Unit.HealthPointsMax;
    }

    // Comprobamos el tiempo que queda para que el NPC haga respown.

    // TODO: En esta fase, los enemigos deben ignorar a los 
    // NPCs que est� en proceso de respown.
    public override void CheckState(NPC npc)
    {
        if (_timeRespown <= 0)
            RespownUnit(npc);
        else
            _timeRespown--;
    }

    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }

}
