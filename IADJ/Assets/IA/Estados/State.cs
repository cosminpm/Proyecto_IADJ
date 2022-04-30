using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 


public abstract class State : MonoBehaviour
{

     // Objetivo de las acciones del estado
    protected NPC _npc;

    // Indica si el npc está en movimiento
    protected bool movement;


    public State(){
        _npc = null;
        movement = false;
    }

   
    
    public abstract void Action(NPC _npc);  

    public abstract void EntryAction(NPC _npc);

    public abstract void ExitAction(NPC _npc);

    public abstract void CheckState(NPC _npc);


    // Comprobaciones

    protected bool isDead (NPC npc){

        if ( npc.Unit.CurrentHealthPoints <= 0){
            npc.ChangeState(npc.stateDead);
            return true;
        }
        return false;
    }

    // Transiciones






}