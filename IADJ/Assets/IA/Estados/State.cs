using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using UnityEngine.UI;
using Global;

public abstract class State : MonoBehaviour
{

     // Objetivo de las acciones del estado
    protected NPC _targetNPC;

    // Indica si el npc está en movimiento
    protected bool movement;

    // Imagen del estado
    public Image stateImage;

    public State(){
        _targetNPC = null;
        movement = false;
    }

    void Awake(){
        stateImage.enabled = false;
    }
 
    public abstract void Action(NPC npc);  

    public abstract void EntryAction(NPC npc);

    public abstract void ExitAction(NPC npc);

    public abstract void CheckState(NPC npc);

    public abstract void Execute(NPC npc);


    // Funciones auxiliares

    public NPC ObjetiveNPC
    {
        get { return _targetNPC; }
        set { _targetNPC = value; }
    }



}