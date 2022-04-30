using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class NPC : MonoBehaviour
{


    // Tipo de unidad
    private UnitsManager _unit;
    
    // Estados posibles del NPC   
    public Capture stateCapture;
    public Dead stateDead;
    public Healing stateHealing;
    public MeleeAttack stateMeleeAtack;
    public RangeAttack stateRangeAttack;

    // Movimiento del personaje
    private PathFinding pathFinding;

    // Estado actual del NPC
    private State _currentState;



    void Awake(){
        _unit = GetComponent<UnitsManager>();
    }

    void Start(){
        
    }

    void Update(){
       
    }


    public void ChangeState(State newState){

    }
    


    public UnitsManager Unit
    {
        get { return _unit; }
        set { _unit = value; }
    }

    public Vector3 GetUnitPosition(){

        return Unit.UnitAgent.Position;
    }

}

   