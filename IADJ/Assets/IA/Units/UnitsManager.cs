using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public abstract class UnitsManager : MonoBehaviour
{

      public enum Team{
        Neutral = 0,
        Red = 1,
        Blue = 2    
    }

    // Bando
    [SerializeField] private Team team;

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


    // Velocidad de movimiento
    private int _movementSpeed;

    private int prueba;

    private float _maxSpeed;
    private float _maxRotation;
    private float _maxAcceleration;
    private float _maxAngularAcc;
    private float _speed; // velocidad escalar
    private Vector3 _acceleration; // aceleración lineal
    private float _rotation;

    
    

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

    






}

   