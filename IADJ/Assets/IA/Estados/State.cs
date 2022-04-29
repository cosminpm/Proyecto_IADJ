using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 


public abstract class State : MonoBehaviour
{

     // Objetivo de las acciones del estado
    private NPC _npc;

    // Indica si el npc está en movimiento
    private bool _movement;


    public State(){
        _npc = null;
        _movement = false;
    }

   
    
    public abstract void GetAction(NPC _npc);  

    public abstract void GetEntryAction(NPC _npc);

    public abstract void GetExitAction(NPC _npc);

    public abstract void CheckState(NPC _npc);


    // Comprobaciones

    protected bool isDead (NPC npc){

        if ( npc.PhMax <= 0){
            npc.ChangeState(npc.stateDead);
            return true;
        }
        return false;
    }

    // Transiciones






}