using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : State
{
    // Variable para comprobar, en frames, el tiempo de respown.
    // 600 frames = 10s
    private int _timeRespown = 600;
    private bool _dead = false ;


    void Awake(){

        stateImage.enabled = false;
    }


    public override void EntryAction(NPC npc)
    {
        
    }

    public override void ExitAction(NPC npc)
    {
        npc.Unit.CurrentHealthPoints = npc.Unit.HealthPointsMax;
    }

    // Cambiamos la posici�n del NPC y hacemos que se cure por completo.
    public override void Action(NPC npc)
    {

        if (_timeRespown <= 450 && !_dead){
            npc.Respawn();
            _dead = true;
        }
    }

    // Comprobamos el tiempo que queda para que el NPC haga respown.

    // TODO: En esta fase, los enemigos deben ignorar a los 
    // NPCs que est� en proceso de respown.
    public override void CheckState(NPC npc)
    {
        if (_timeRespown <= 0)
            npc.stateManager.RespawnUnit();
        else
            _timeRespown--;
    }

    public override void Execute(NPC npc)
    {
        Action(npc);
        CheckState(npc);
    }

}
