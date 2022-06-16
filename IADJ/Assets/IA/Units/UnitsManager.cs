using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEditor;



public abstract class UnitsManager : MonoBehaviour
{

    public enum Team {
        Red = 0,
        Blue = 1    
    }

    // Bando
    [SerializeField] private Team _team;
    [SerializeField] public bool _drawGizmosAux;

    // CARACTERÍSTICAS DEL NPC
    // Puntos de vida Máximos
    private float _hpMax;
    // Puntos de vida críticos
    private float _hpMin;
    // Puntos de vida
    private float _hp;
    // Puntos de ataque
    private float _pAttack;
    // Rango de ataque
    private float _range;
    // Velocidad de ataque
    private int _attackSpeed;
    // Precisión de ataque
    private float _attackAccuracy;
    // Tasa de crítico
    private float _criticRate;
    // Rango de visión
    private float _vision;
    // Valor de la influencia que ejerce sobre el mapa
    private float _influence;


    protected AgentNPC _agentNPC;

    protected float _maxSpeed;
    
    // // Tipos de terrenos.
    // public enum TypeTerrains{
    //     Path = 0,
    //     Field = 1,
    //     Forest = 2,
    //     Cliff = 3,  
    //     River = 4
    // }

    // Diccionario con los costes de movimiento de cada tipo de terrenos.
    protected Dictionary<GridMap.TipoTerreno, float> costsTerrains;

    // Tipos de unidad.
    public enum TypeUnits{
        Soldier = 0,
        Archer = 1,
        Tank = 2,
        Healer = 3
    }

    // Modos de comportamiento de la unidad. 
    public enum Modes { 
        Normal = 0,
        Offensive = 1,
        Defensive = 2,
        TotalWar = 3
    }

    // Tipo de unidad
    private TypeUnits _unitType;

    // Modo de comportamiento de la unidad
    private Modes _unitMode;

    private float _bonusSpeed = 2;


    protected abstract void AddCostsTerrain();
    protected abstract void SetMovementStats();
    protected abstract void SetUnitStats();

    // Funcion para activar el modo TotalWar.
    public abstract void ActivateTotalWar();

    // Funcion para activar el modo Offensive.
    public abstract void ActivateOffensiveMode();

    // Funcion para activar el modo Defensive.
    public abstract void ActivateDefensiveMode();

    // Funcion para activar el modo Normal.
    public abstract void ActivateNormalMode();

    public void ActivateSpeedBonus(){
        UnitAgent.MaxSpeed += _bonusSpeed; 
        _maxSpeed += _bonusSpeed;
    }


    public void DeactivateSpeedBonus(){
        UnitAgent.MaxSpeed -= _bonusSpeed; 
        _maxSpeed -= _bonusSpeed;
    }

    public void SetCostTerrainSpeed(GridMap.TipoTerreno terrain){

        float coste;
        costsTerrains.TryGetValue(terrain,out coste);
        if ( terrain == GridMap.TipoTerreno.Camino  || terrain == GridMap.TipoTerreno.BaseAzul || terrain == GridMap.TipoTerreno.BaseRoja){
         
           // Debug.Log("HOAHODADOAHODA Y mi max speed es "+UnitAgent.MaxSpeed);
             UnitAgent.MaxSpeed = _maxSpeed;
        }

        else {
            if ( _maxSpeed != _maxSpeed* (1-coste) )
                UnitAgent.MaxSpeed = _maxSpeed* (1-coste);
        }
    }

    public float GetMovementCost(GridMap.TipoTerreno terrain){
        float cost;
        costsTerrains.TryGetValue(terrain,out cost);
        return cost;
    }

    public float GetInfluence(){
        return _influence;
    }

    public float HealthPointsMax
    {
        get { return _hpMax; }
        set { _hpMax = value; }
    }

    public float HealthPointsMin
    {
        get { return _hpMin; }
        set { _hpMin = value; }
    }

    public float CurrentHealthPoints
    {
        get { return _hp; }
        set { 
            if ( value >= HealthPointsMax)
                _hp = HealthPointsMax;
            else 
                _hp = value;
        }
    }

    public float AttackPoints
    {
        get { return _pAttack; }
        set { _pAttack = value; }
    }

    public float AttackRange
    {
        get { return _range; }
        set { _range = value; }
    }

    public int AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    public float AttackAccuracy
    {
        get { return _attackAccuracy; }
        set { _attackAccuracy = value; }
    }
    public float CriticRate
    {
        get { return _criticRate; }
        set { _criticRate = value; }
    }

    public float VisionDistance
    {
        get { return _vision; }
        set { _vision = value; }
    }

    public float Influence
    {
        get {return _influence;}
        set { _influence = value; }
    }

    public Team UnitTeam
    {
        get { return _team; }
        set { _team = value; }
    }

    public TypeUnits TypeUnit
    {
        get { return _unitType; }
        set { _unitType = value; }
    }

    public Modes Mode {
        get { return _unitMode; }
        set { _unitMode = value; }
    } 

    public AgentNPC UnitAgent{
        get { return _agentNPC; }
        set { _agentNPC = value; }
    }


    public void OnDrawGizmos()
    {
        if (_drawGizmosAux) {
            
            Handles.color = Color.blue;
            Handles.DrawWireDisc(UnitAgent.Position, Vector3.up, VisionDistance, 3);
            Handles.color = Color.red;
            Handles.DrawWireDisc(UnitAgent.Position, Vector3.up, AttackRange, 5);
        }

    }


}

   