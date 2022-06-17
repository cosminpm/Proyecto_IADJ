using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
public class StateManager: MonoBehaviour
{

    // Estados posibles del NPC   
    public Capture stateCapture;
    public Dead stateDead;
    public Patroll statePatroll;
    public LowHp stateLowHp;
    public Attack stateAttack;
    public Defend stateDefend;
    public Protect stateProtect;
    public Healing stateHealing;
    public ReceivingHeal stateReceivingHeal;
    public RangeAttack stateRangeAttack;
    public Order stateOrder;

    // Estado actual del npc
    private State _currentState;

    // NPC
    private NPC _npc;

    public void Initialize(int type, NPC npc){
        
        stateCapture = this.gameObject.GetComponent<Capture>();
        stateDead = this.gameObject.GetComponent<Dead>();
        stateLowHp = this.gameObject.GetComponent<LowHp>();
        stateAttack = this.gameObject.GetComponent<Attack>();
        stateDefend = this.gameObject.GetComponent<Defend>();
        stateProtect = this.gameObject.GetComponent<Protect>();
        stateHealing = this.gameObject.GetComponent<Healing>();
        stateReceivingHeal = this.gameObject.GetComponent<ReceivingHeal>();
        stateRangeAttack = this.gameObject.GetComponent<RangeAttack>();
        statePatroll = this.gameObject.GetComponent<Patroll>();
        stateOrder = this.gameObject.GetComponent<Order>();
        _npc = npc;
        
        if (type == 3)
        {
            ChangeState(statePatroll);
        }
        else
            ChangeState(stateCapture);

        _currentState.stateImage.enabled = true;
     
    }

    public void Execute(NPC npc){
        if ( _currentState != null)
            _currentState.Execute(npc); 
    }

    // Función para cambiar el estado del NPC
    public void ChangeState(State newState){

        if ( _currentState != null && _currentState != newState)
        {
            _currentState.ExitAction(_npc);
        }

        if (_currentState != newState) {
            _npc.GUI.UpdateStateImagen(_currentState,newState);
            _currentState = newState;
            _currentState.EntryAction(_npc);
        } 
    }

    // Función para comprobar si un NPC ha acabado de
    // curarse. Si ese es el caso, pasará a estado Capture.
    public bool HealingFinished() {
        if (_npc.GetUnitCurrentHP() >= _npc.GetUnitHPMax()) 
        {
            ChangeState(stateCapture);
            return true;
        }
        return false;
    }

    // Función para comprobar si el Healer ha acabado de 
    // curar a un NPC.
    public bool CureFinished(NPC targetNPC) {
        if (targetNPC.IsFullHP() || targetNPC.IsCurrentStateDead() ) {
            ChangeState(statePatroll);
            return true;
        }
        return false;
    }


    public bool HealthPointReached(bool finalPath ){

        if ( finalPath){
            ChangeState(stateReceivingHeal);
            return true;
        }
        return false;
    }

    // Función para comprobar que si el NPC necesita curación.
    // Si ese es el caso, se cambiará al estado LowHp.
    public bool IsLowHP() {

        if (_npc.NeedHeal() && !_npc.IsTotalWar()) {
            ChangeState(stateLowHp);
            return true;
        }
        return false;
    }

    // Función para comprobar si el NPC está muerto. 
    // Si ese es el caso, pasará al estado Dead.
    public bool IsDead() {
        
        if ( _npc.Unit.CurrentHealthPoints <= 0){
            ChangeState(stateDead);
            return true;
        }
        return false;
    }

    public bool EnemyFound(){
        
        List<NPC> enemies = _npc.GameManager.FindNearbyEnemies(_npc);

        if (enemies.Count > 0) {
            NPC target = _npc.FindClosestEnemy(enemies);

            if ( _npc.GetUnitType() != (int) UnitsManager.TypeUnits.Archer ){
                stateAttack.ObjetiveNPC = target;
                Debug.Log("Ento aquí");
                ChangeState(_npc.stateManager.stateAttack);
            } else {
                  _npc.stateManager.stateRangeAttack.ObjetiveNPC = target;
                 ChangeState(_npc.stateManager.stateRangeAttack);
            }

            return true;
        } 

        if ( _currentState != stateDefend)
           ChangeState(_npc.stateManager.stateCapture);
        return false;

    }

     
        // Función para respawnear a un NPC.
    public void RespawnUnit() {
        ChangeState(stateCapture);
        return;
    }

    // Función para comprobar que un NPC esté en el modo total war
    public bool TotalWar(){
        if (!_npc.IsTotalWar() && _npc.GameModeIsTotalWar()){
            Debug.Log("AAAAAAAA");
            ChangeState(stateCapture);
            return true;
        }
        return false;
    }

    
    // Función para comprobar si un npc tiene que ir 
    // a socorrer a un aliado.
    public bool BackupNeeded()
    {
        if (_npc.GetUnitType() == (int)UnitsManager.TypeUnits.Tank)
        {
            NPC target = _npc.FindClosestHurtedAllie();

            if (target != null)
            {
                stateProtect.ObjetiveNPC = target;
                ChangeState(stateProtect);
                return true;
            }

        }
        return false;
    }

    // Están capturando mi base. Y si estoy a cierta distancia, voy a defender
    public bool IsCapturingBase(){

        Vector3 zonaEnemiga = _npc.GameManager.waypointManager.GetEnemyZonePosition(_npc);
        Vector3 basePropia = _npc.GameManager.waypointManager.GetBasePosition(_npc);


        float distanciaZonaEnemiga = Vector3.Distance(zonaEnemiga, _npc.GetUnitPosition());
        float distanciaBase = Vector3.Distance(basePropia , _npc.GetUnitPosition());


        switch ( (int) _npc.GetUnitTeam() )
        {
            case (int) GlobalAttributes.Team.Blue:
               if (_npc.GameManager.CapturingRedTeam() && distanciaBase < distanciaZonaEnemiga){
                    ChangeState(stateDefend);
                    return true;
               }
                return false;

                break;
            case (int) GlobalAttributes.Team.Red:
                if (_npc.GameManager.CapturingBlueTeam() && distanciaBase < distanciaZonaEnemiga){
                    ChangeState(stateDefend);
                    return true;
                }
                return false;
                break;
            default:
                return false;
                break;
        }

    }

    // Función para comprobar si un aliado necesita curación
    public bool AllieHealthReached(){

        List<NPC> listAllies = _npc.GameManager.FindNearbyAllies(_npc);

        if ( listAllies.Count > 0)
        {
            foreach( var a in listAllies)
            {
                if (a.NeedHeal())
                {
                    ChangeState(stateHealing);
                    CurrentState.ObjetiveNPC = a;
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

    public bool CurrentStateIsReceivingHealing(){
        return CurrentState == stateReceivingHeal;
    }
}
