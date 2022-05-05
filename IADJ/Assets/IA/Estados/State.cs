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


    // Transiciones TODO: JAJAJAJAJAJ

    // Función para comprobar si el NPC está muerto. 
    // Si ese es el caso, pasará al estado Dead.
    protected bool IsDead (NPC npc){
        
        if ( npc.Unit.CurrentHealthPoints <= 0){
            npc.stateManager.ChangeState(npc.stateManager.stateDead, npc);
            return true;
        }
        return false;
    }

    // TODO: Esto a lo mejor hay que moverlo a StateManager.
    //
    // Función para comprobar si hay enemigos cerca. 
    // Si ese es el caso, pasará a estado Attack.
    protected bool EnemyFound(NPC npc){
        
        if (FindClosestEnemy(npc))
        {
            npc.stateManager.stateAttack.ObjetiveNPC = npc.stateManager.CurrentState.ObjetiveNPC;
            npc.stateManager.ChangeState(npc.stateManager.stateAttack, npc);
            return true;
        }

        else
            npc.stateManager.ChangeState(npc.stateManager.stateCapture, npc);

        return false;
    }


    // Función para respownear a un NPC.
    protected void RespownUnit(NPC npc) {
        npc.stateManager.ChangeState(npc.stateManager.stateCapture, npc);
        return;
    }

    // Función para comprobar si el NPC está en una zona de 
    // curación y cambiar al estado Healing en ese caso.
    protected bool IsHealing(NPC npc) {
        return false;
    }


    // Funciones auxiliares


    // Encuentro el enemigo más cercano dentro de mi rango de visión. Me gusta más en el NPC JULIO 
    private bool FindClosestEnemy(NPC npc){

        // Lista de enemigos
        GameObject[] enemys;

        // Dependiendo del equipo que sea, busco mis enemigos
        switch ((int) npc.Unit.UnitTeam)
        {
            case 0:
                enemys = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL);
                break;
            case 1:
                enemys = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO);
                break;
            default:
                enemys = null;
                break;
        }

         // Obtenemos la dirección hacia el target
        Vector3 direction;
        // Distancia que hay entre el agente y el target
        float distance;
        float minDistance = Mathf.Infinity;

        NPC auxEnemy = null;

        // Recorro todos mis enemigos y me quedo con el más cercano si está en mi rango de visión.
        foreach ( var e in enemys)
        {
            direction = e.transform.position - npc.GetUnitPosition();
            distance = direction.magnitude;

            if ( distance < minDistance && distance <= npc.Unit.VisionDistance)
            {
                auxEnemy = e.GetComponent<NPC>();
                minDistance = distance;
            } 
        }

        if ( auxEnemy != null ){
            _targetNPC = auxEnemy;
            return true;
        
        }
        return false;

    }

    public NPC ObjetiveNPC
    {
        get { return _targetNPC; }
        set { _targetNPC = value; }
    }

    // Transiciones






}