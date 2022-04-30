using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Capture : State
{

    public override void Action(NPC npc, NPC obj){
        
    }

    public override void EntryAction(NPC npc){
        
    }

    public override void ExitAction(NPC npc){
       // movement = false;
       // _targetNPC = null;
    }

    public override void CheckState(NPC npc){

        // Si estoy muerto, cambio al estado a muerti
        if  ( IsDead(npc) ){

            
             return;
        }

        // Si hay algún al que atacar, cambio de estado a MeleeAttack
        if ( EnemyFound(npc) ){
            return;
        }

       // npc.ChangeState(npc.stateCapture);


    }

    public override void Execute(NPC npc){
        Action(npc, _targetNPC);
        CheckState(npc);
        
    }


}