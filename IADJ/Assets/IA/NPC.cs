using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class NPC : MonoBehaviour
{

    // Estados posibles del NPC   
    public Capture stateCapture;
    public Dead stateDead;
    public Healing stateHealing;
    public MeleeAttack stateMeleeAtack;
    public RangeAttack stateRangeAttack;

    // CARACTERÍSTICAS DEL NPC

    // Puntos de vida Máximos
    private int _phMax;
    // Puntos de vida
    private int _ph;
    // Puntos de ataque
    private int _pAttack;
    // Rango de ataque
    private int _range;
    // Velocidad de ataque
    private int _attackSpeed;
    // Precisión de ataque
    private int _hitRate;
    

    // Movimiento del personaje
    private AgentNPC _agentNPC;
    private PathFinding pathFinding;

    // Estado actual del NPC
    private State _currentState;





    void Update(){

    }


    public void ChangeState(State newState){

    }

    // Getters y Setters;

    public int PhMax
    {
        set { _phMax = value; }
        get { return _phMax; }
    }

    public State CurrentState
    {
        set { _currentState = value; }
        get { return _currentState; }
    }


}

   