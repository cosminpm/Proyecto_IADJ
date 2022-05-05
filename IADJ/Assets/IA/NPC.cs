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

    // Movimiento del personaje
    private PathFinding pathFinding;

    // GUI NPC
    public GUIManager GUI;


    // Manejador de combate
    void Awake(){
        _unit = GetComponent<UnitsManager>();
        stateManager = GetComponent<StateManager>();
        GUI = GetComponent<GUIManager>();
        GUI.Initialize();
        stateManager.Initialize(GetUnitType(), this);
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
            if ( this.isInVisionRange(a.GetComponent<NPC>()))
                 listAllies.Add(a.GetComponent<NPC>());
        }

        return listAllies;
    }


    public bool isInVisionRange(NPC npc){

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

    public bool NeedHeal()
    {
        return GetUnitCurrentHP() <= GetUnitHPMin();
    }


    public UnitsManager Unit
    {
        get { return _unit; }
        set { _unit = value; }
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

   