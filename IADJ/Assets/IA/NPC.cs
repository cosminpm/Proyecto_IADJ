using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Global;

public class NPC : MonoBehaviour
{


    // Tipo de unidad
    private UnitsManager _unit;
    
    // Controlador de estados
    public StateManager stateManager;

    // GUI NPC
    public GUIManager GUI;

    // Controlador pathfinding
    public ControlPathFindingWithSteering pathFinding;

    // DUDA AQUI ABAJO HACE FALTA??
    // GameManager
    private GameHandler _gameManager;


    // Manejador de combate
    void Awake(){
        _unit = GetComponent<UnitsManager>();
        stateManager = GetComponent<StateManager>();
        GUI = GetComponent<GUIManager>();
        GUI.Initialize();
        stateManager.Initialize(GetUnitType(), this);
        pathFinding = GetComponent<ControlPathFindingWithSteering>();
    }

    void Start(){
        
    }

    void Update(){

        // Si el NPC está en un estado, ejecuta la acción correspondiente
        stateManager.Execute(this); 

    }

    public List<NPC> FindNearbyAllies(){

       // Lista de aliados
        GameObject[] allies;
        List<NPC> listAllies = new List<NPC>();

        // Dependiendo del equipo que sea, busco mis amigos
        switch ((int) GetUnitTeam())
        {
            case 0:
                allies = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO);
                break;
            case 1:
                allies = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL);
                break;
            default:
                allies = null;
                break;
        }

        foreach ( var a in allies){
            if ( this.IsInVisionRange(a.GetComponent<NPC>()))
                 listAllies.Add(a.GetComponent<NPC>());
        }

        return listAllies;
    }


    public bool IsInVisionRange(NPC npc){

        if (npc == null)
            Debug.Log(" DEsauno con juevboi");

        // Obtenemos la dirección hacia el target
        Vector3 direction = npc.GetUnitPosition() - this.GetUnitPosition();
        // Distancia que hay entre el agente y el target
        float distance = direction.magnitude;

        if ( distance <= GetUnitVisionDistance() ){
            return true;
        }
        return false;
    }

    public void Respawn(){
        this.Unit.UnitAgent.Position = _gameManager.waypointManager.GetBasePosition(this);
    }

    public bool IsFullHP(){
        return GetUnitCurrentHP() >= GetUnitHPMax();
    }

    public bool NeedHeal()
    {
        return GetUnitCurrentHP() <= GetUnitHPMin();
    }

    public bool IsCurrentStateDead(){
        return stateManager.CurrentStateIsDead();
    }
    
    public bool IsCapturing(){

        if ( stateManager.CurrentStateIsCapture() && _gameManager.waypointManager.InsideWaypoint(this,_gameManager.waypointManager.GetEnemyZone(this))) 
            return true;

        return false;

        // if ( stateManager.CurrentStateIsCapture())
        //     Debug.Log("ESTOY EN EL ESTADO ATACAR ES TRUEE EJIJIJI");
        
        // if (_gameManager.waypointManager.InsideWaypoint(this,_gameManager.waypointManager.GetEnemyZone(this))) 
        //     Debug.Log("He llegado al destino buaajjaja");

      //  return false;

    }

    public bool IsBase(){
        
        return _gameManager.waypointManager.InsideWaypoint(this,_gameManager.waypointManager.GetBase(this));

    }



    public UnitsManager Unit
    {
        get { return _unit; }
        set { _unit = value; }
    }

    public GameHandler GameManager
    {
        get { return _gameManager; }
        set { _gameManager = value; }
    }

    public Vector3 GetUnitPosition(){
        return Unit.UnitAgent.Position;
    }

    public float GetUnitVisionDistance(){

        return Unit.VisionDistance;
    }

    public float GetUnitHPMax(){

        return Unit.HealthPointsMax;
    }

    public float GetAttackPoints() {
        return Unit.AttackPoints;
    }

    public float GetUnitHPMin(){

        return Unit.HealthPointsMin;
    }

    public float GetUnitCurrentHP(){
        return Unit.CurrentHealthPoints;
    }

    public int GetUnitType(){
        return (int) Unit.TypeUnit;
    }

    public int GetUnitTeam(){
        return (int) Unit.UnitTeam;
    }

    public float GetNPCArrivalRadius(){
        return Unit.UnitAgent.ArrivalRadius;
    }
}

   