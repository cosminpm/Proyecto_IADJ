using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 


public abstract class State : MonoBehaviour
{

     // Objetivo de las acciones del estado
    public NPC _targetNPC;

    // Indica si el npc está en movimiento
    protected bool movement;


    public State(){
        _targetNPC = null;
        movement = false;
    }

   
    
    public abstract void Action(NPC npc, NPC obj);  

    public abstract void EntryAction(NPC npc);

    public abstract void ExitAction(NPC npc);

    public abstract void CheckState(NPC npc);

    public abstract void Execute(NPC npc);


    // Transiciones 

    // Función para comprobar si el NPC está muerto. 
    // Si ese es el caso, pasará al estado Dead.
    protected bool IsDead (NPC npc){

        if ( npc.Unit.CurrentHealthPoints <= 0){
            npc.ChangeState(npc.stateDead);
            return true;
        }
        return false;
    }

    // Función para comprobar que si el NPC necesita curación.
    // Si ese es el caso, se cambiará al estado LowHp.
    protected bool NeedHeal(NPC npc) {

        if (npc.Unit.CurrentHealthPoints <= npc.Unit.HealthPointsMin) {
            npc.ChangeState(npc.stateLowHp);
            return true;
        }

        return false;
    }

    // Función para comprobar si hay enemigos cerca. 
    // Si ese es el caso, pasará a estado Attack.
    protected bool EnemyFound(NPC npc){

        if (findClosestEnemy(npc))
        {
            npc.stateAttack.ObjetiveNPC = npc.CurrentState.ObjetiveNPC;
            npc.ChangeState(npc.stateAttack);
            return true;
        }

        else
            npc.ChangeState(npc.stateCapture);

        return false;
    }

    // Función para respownear a un NPC.
    protected void RespownUnit(NPC npc) {
        npc.ChangeState(npc.stateCapture);
        return;
    }

    // Función para comprobar si el NPC está en una zona de 
    // curación y cambiar al estado Healing en ese caso.
    protected bool IsHealing(NPC npc) {
        return false;
    }

    // Función para comprobar si un NPC ha acabado de
    // curarse. Si ese es el caso, pasará a estado Capture.
    protected bool HealingFinished(NPC npc) {
        if (npc.Unit.CurrentHealthPoints >= npc.Unit.HealthPointsMax) {
            npc.ChangeState(npc.stateCapture);
            return true;
        }
        return false;
    }

    // Funciones auxiliares


    // Encuentro el enemigo más cercano dentro de mi rango de visión. Me gusta más en el NPC JULIO 
    private bool findClosestEnemy(NPC npc){

        // Lista de enemigos
        GameObject[] enemys;

        // Dependiendo del equipo que sea, busco mis enemigos
        switch ((int) npc.Unit.UnitTeam)
        {
            case 0:
                enemys = GameObject.FindGameObjectsWithTag("TeamBlue");
                break;
            case 1:
                enemys = GameObject.FindGameObjectsWithTag("TeamRed");
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