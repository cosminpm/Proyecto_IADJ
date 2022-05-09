using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager: MonoBehaviour
{

    // TODO: Variable de debug Defend. ELIMINAR.
    int framesToDefend = 120;

    // Estados posibles del NPC   
    public Capture stateCapture;
    public Dead stateDead;
    public LowHp stateLowHp;
    public Attack stateAttack;
    public Defend stateDefend;
    public Healing stateHealing;
    public ReceivingHeal stateReceivingHeal;
    public SearchHealing stateSearchHealing;


    // Estado Actual del npc
    private State _currentState;

    void Awake(){

    //     stateCapture = this.gameObject.GetComponent<Capture>();
    //     stateDead = this.gameObject.GetComponent<Dead>();
    //     stateLowHp = this.gameObject.GetComponent<LowHp>();
    //     stateAttack = this.gameObject.GetComponent<Attack>();
    //     stateHealing = this.gameObject.GetComponent<Healing>();
    //     stateReceivingHeal = this.gameObject.GetComponent<ReceivingHeal>();
    //     stateSearchHealing = this.gameObject.GetComponent<SearchHealing>();
    //    // stateSearchHealing = this.gameObject.GetComponent<ReceivingHeal>();
    
    //     _currentState = stateCapture;
    //     _currentState.stateImage.enabled = true;
    }

    public void Initialize(int type, NPC npc){
        
        stateCapture = this.gameObject.GetComponent<Capture>();
        stateDead = this.gameObject.GetComponent<Dead>();
        stateLowHp = this.gameObject.GetComponent<LowHp>();
        stateAttack = this.gameObject.GetComponent<Attack>();
        stateDefend = this.gameObject.GetComponent<Defend>();
        stateHealing = this.gameObject.GetComponent<Healing>();
        stateReceivingHeal = this.gameObject.GetComponent<ReceivingHeal>();
        stateSearchHealing = this.gameObject.GetComponent<SearchHealing>();
    
        if ( type == 3)  
            ChangeState(stateSearchHealing, npc);
        else    
            ChangeState(stateCapture, npc);

        _currentState.stateImage.enabled = true;
     
    }

    public void Execute(NPC npc){
        if ( _currentState != null)
            _currentState.Execute(npc); 
    }

    // Función para cambiar el estado del NPC
    public void ChangeState(State newState, NPC npc){

       
        if ( _currentState != null && _currentState != newState)
        {
            _currentState.ExitAction(npc);
        }

        if (_currentState != newState) {
            npc.GUI.UpdateStateImagen(_currentState,newState);
            _currentState = newState;
            _currentState.EntryAction(npc);
        } 
    }

    // Función para comprobar si hay enemigos en la base.
    public bool EnemiesInBase(NPC npc) {

        // TODO: Comprobar que van perdiendo (GameManager).
        // TODO: Si van perdiendo, se comprueba si hay enemigos
        //       en la base (mapa de influencia).
        if (false)//framesToDefend == 0)
        {
            ChangeState(stateDefend, npc);
            return true;
        }

        //framesToDefend--;
        return false;
    }

    // Función para comprobar si un NPC ha acabado de
    // curarse. Si ese es el caso, pasará a estado Capture.
    public bool HealingFinished(NPC npc) {
        if (npc.GetUnitCurrentHP() >= npc.GetUnitHPMax()) 
        {
            ChangeState(stateCapture, npc);
            return true;
        }
        return false;
    }

    // Función para comprobar si el Healer ha acabado de 
    // curar a un NPC.
    public bool CureFinished(NPC npc, NPC targetNPC) {
        if (targetNPC.IsFullHP()) {
            ChangeState(stateSearchHealing, npc);
            return true;
        }
        return false;
    }

    // public bool HealthPointReached(Vector3 posBase, NPC npc, NPC healer, bool medicFound, ){

    //     // Obtenemos la dirección hacia el target
    //     Vector3 direction;
    //     // Distancia que hay entre el agente y el target
    //     float distance;
        
    //     if ( medicFound)
    //     {
    //         direction = healer.GetUnitPosition() - npc.GetUnitPosition();
    //         distance = direction.magnitude;
    //         if (  distance <= healer.GetNPCArrivalRadius()){
    //             ChangeState(stateReceivingHeal, npc);
    //             Debug.Log("Voy pal pollo payo");
    //             return true;
    //         }

    //     } else {
    //         direction = posBase - npc.GetUnitPosition();
    //         distance = direction.magnitude;
    //         if (  distance == 0 ){
    //             Debug.Log("Me voy pa la base amigo0");
    //             ChangeState(stateReceivingHeal, npc);
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    public bool HealthPointReached( NPC npc, bool finalPath ){

        if ( finalPath){
            ChangeState(stateReceivingHeal, npc);
            return true;
        }
        return false;
    }

    // Función para comprobar que si el NPC necesita curación.
    // Si ese es el caso, se cambiará al estado LowHp.
    public bool IsLowHP(NPC npc) {

        if (npc.NeedHeal()) {
            npc.stateManager.ChangeState(npc.stateManager.stateLowHp, npc);
            return true;
        }
        return false;
    }
     
        // Función para respawnear a un NPC.
    public void RespawnUnit(NPC npc) {
        npc.stateManager.ChangeState(npc.stateManager.stateCapture, npc);
        return;
    }

    public bool AllieHealthReached(NPC npc){

        List<NPC> listAllies = npc.GameManager.FindNearbyAllies(npc);

        if ( listAllies.Count > 0)
        {
            foreach( var a in listAllies)
            {
                if (a.NeedHeal())
                {
                    ChangeState(stateHealing, npc);
                    CurrentState._targetNPC = a;
                    return true;
                }
            }
        }
        return false;
    }

    public State CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

    public bool CurrentStateIsDead(){
        return CurrentState == stateDead;
    }

    public bool CurrentStateIsCapture(){
        return CurrentState == stateCapture;
    }

    public NPC GetStateTarget(){
        return CurrentState.ObjetiveNPC;
    }

    public void SetStateAttackTarget(NPC npc){
        stateAttack.SetObjetiveNPC(npc);
    }     
}
