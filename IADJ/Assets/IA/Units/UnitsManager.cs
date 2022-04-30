using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public abstract class UnitsManager : MonoBehaviour
{

    public enum Team{
        Red = 0,
        Blue = 1    
    }

    // Bando
    [SerializeField] private Team _team;


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
    private float _attackSpeed;
    // Precisión de ataque
    private float _attackAccuracy;
    // Rango de visión
    private float _vision;


    protected AgentNPC _agentNPC;

    // Velocidad de movimiento
    private int _movementSpeed;

    private float _maxSpeed;
    private float _maxRotation;
    private float _maxAcceleration;
    private float _maxAngularAcc;
    private float _speed; // velocidad escalar

    
    // Tipos de terrenos.
    public enum TypeTerrains{

        Path = 0,
        Field = 1,
        Forest = 2,
        Cliff = 3,  
        River = 4
    }

    // Diccionario con los costes de movimiento de cada tipo de terrenos.
    protected Dictionary<TypeTerrains, float> costsTerrains;


    public enum TypeUnits{
        Soldier = 0,
        Archer = 1,
        Tank = 2,
    }

      // Tipo de unidad
    private TypeUnits _unitType;

    // Diccionaro con los costes de daño a cada tipo de unidad
    protected Dictionary<TypeUnits, float> mapUnitDamage;




    protected abstract void AddCostsTerrain();
    protected abstract void AddUnitDamage();
    protected abstract void SetMovementStats();
    protected abstract void SetUnitStats();


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
        set { _hp = value; }
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

    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    public float AttackAccuracy
    {
        get { return _attackAccuracy; }
        set { _attackAccuracy = value; }
    }

    public float VisionDistance
    {
        get { return _vision; }
        set { _vision = value; }
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

    public AgentNPC UnitAgent{
        get { return _agentNPC; }
        set { _agentNPC = value; }
    }


    






}

   