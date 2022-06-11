using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using UnityEngine.UI;
using Global;

public abstract class State : MonoBehaviour
{

     // Objetivo de las acciones del estado
    public NPC _targetNPC;

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
 
    public abstract void Action(NPC npc, NPC obj);  

    public abstract void EntryAction(NPC npc);

    public abstract void ExitAction(NPC npc);

    public abstract void CheckState(NPC npc);

    public abstract void Execute(NPC npc);


    // Función para comprobar si el NPC está en TotalWar.
    protected bool IsInTotalWar(NPC npc)
    {
        if (npc.Unit.Mode == UnitsManager.Modes.TotalWar)
        {
            npc.stateManager.ChangeState(npc.stateManager.stateCapture, npc);
            return true;
        }
        return false;
    }

    // TODO: Esto a lo mejor hay que moverlo a StateManager.
    //
    // Función para comprobar si hay enemigos cerca. 
    // Si ese es el caso, pasará a estado Attack.
    protected bool EnemyFound(NPC npc){
        
        List<NPC> enemies = npc.GameManager.FindNearbyEnemies(npc);


        if (enemies.Count > 0) {

            NPC target = npc.FindClosestEnemy(enemies);

            if ( npc.GetUnitType() != (int) UnitsManager.TypeUnits.Archer ){
                npc.stateManager.stateAttack.ObjetiveNPC = target;
                npc.stateManager.ChangeState(npc.stateManager.stateAttack, npc);
            } else {
                  npc.stateManager.stateRangeAttack.ObjetiveNPC = target;
                  npc.stateManager.ChangeState(npc.stateManager.stateRangeAttack, npc);
            }

            return true;
        } 

        npc.stateManager.ChangeState(npc.stateManager.stateCapture, npc);
        return false;

    }



    // Función para comprobar si el NPC está en una zona de 
    // curación y cambiar al estado Healing en ese caso.
    protected bool IsHealing(NPC npc) {
        return false;
    }


    // Funciones auxiliares


    public NPC ObjetiveNPC
    {
        get { return _targetNPC; }
        set { _targetNPC = value; }
    }

    // Transiciones






}