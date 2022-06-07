using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Global;
using Grid;

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
    public GameHandler _gameManager;

    // Control Patrullaje
    public bool isPatroll;


    // Manejador de combate
    void Awake(){
        _unit = GetComponent<UnitsManager>();
        stateManager = GetComponent<StateManager>();
        GUI = GetComponent<GUIManager>();
        GUI.Initialize();
        stateManager.Initialize(GetUnitType(), this);
        pathFinding = GetComponent<ControlPathFindingWithSteering>();
        isPatroll = true;
    }

    void Start(){
        
    }

    void Update(){

        // Si el NPC está en un estado, ejecuta la acción correspondiente
        stateManager.Execute(this); 
        ModifyVelocityByTerrain();

    }

    public bool IsInVisionRange(NPC npc){

        // Obtenemos la dirección hacia el target
        Vector3 direction = npc.GetUnitPosition() - this.GetUnitPosition();
        // Distancia que hay entre el agente y el target
        float distance = direction.magnitude;

        if ( distance <= GetUnitVisionDistance() ){
            return true;
        }
        return false;
    }

    // Encuentro el enemigo más cercano dentro de mi rango de visión. 
    public NPC FindClosestEnemy(List<NPC> listEnemy){


         // Obtenemos la dirección hacia el target
        Vector3 direction;
        // Distancia que hay entre el agente y el target
        float distance;
        float minDistance = Mathf.Infinity;

        NPC auxEnemy = null;

        // Recorro todos mis enemigos y me quedo con el más cercano si está en mi rango de visión.
        foreach ( var e in listEnemy)
        {
            direction = e.transform.position - this.GetUnitPosition();
            distance = direction.magnitude;

            if ( distance < minDistance && distance <= this.Unit.VisionDistance)
            {
                if ( !e.GetComponent<NPC>().IsCurrentStateDead() ){
                    auxEnemy = e.GetComponent<NPC>();
                    minDistance = distance;
                }
            } 
        }

        return auxEnemy;
    }

    // Encuentro el aliado más cercano dentro de mi rango de visión. 
    public NPC FindClosestAllie()
    {
        // Obtenemos la lista de aliados
        List<NPC> listAllies = GameManager.FindNearbyAllies(this);
        Debug.Log("ALIADOS CERCANOS: " + listAllies.Count);

        // Obtenemos la dirección hacia el target
        Vector3 direction;
        // Distancia que hay entre el agente y el target
        float distance;
        float minDistance = Mathf.Infinity;

        NPC auxAllie = null;

        // Recorro todos mis aliados y me quedo con el más cercano si está en mi rango de visión.
        foreach (var e in listAllies)
        {
            if (e.name != this.name)
            {
                direction = e.transform.position - this.GetUnitPosition();
                distance = direction.magnitude;

                if (distance < minDistance && distance <= this.Unit.VisionDistance)
                {
                    if (!e.GetComponent<NPC>().IsCurrentStateDead())
                    {
                        auxAllie = e.GetComponent<NPC>();
                        minDistance = distance;
                    }
                }
            }
        }
        return auxAllie;
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

    private void ModifyVelocityByTerrain(){
        Cell cell = pathFinding.WorldToMap(GetUnitPosition());
        Unit.SetCostTerrainSpeed(cell.GetTipoTerreno());
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

    public GlobalAttributes.Team GetEnemyTeam(){

        if ( GetUnitTeam() == (int) GlobalAttributes.Team.Red)
            return GlobalAttributes.Team.Blue;
        else 
            return GlobalAttributes.Team.Red;
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

   