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
   // public Dead stateDead;
   // public Healing stateHealing;
    public Attack stateAttack;


    // Movimiento del personaje
    private PathFinding pathFinding;

    // Estado actual del NPC
    private State _currentState;

    // Manejador de combate


    void Awake(){
        _unit = GetComponent<UnitsManager>();
        // RARO LO DE ABAJO
        stateCapture = this.gameObject.AddComponent<Capture>();
      //  stateDead = new Dead();
      //  stateHealing = new Healing();
        stateAttack = this.gameObject.AddComponent<Attack>();
        ChangeState(stateCapture);
    }

    void Start(){
        
    }

    void Update(){

        // Tengo que realizar la acción del estado

        if ( _currentState != null){ 
            _currentState.Execute(this);
            
        } 

    }


    public void ChangeState(State newState){

        if ( _currentState != null)
        {
            _currentState.ExitAction(this);
        }

        if (_currentState != newState) {


            _currentState = newState;
            _currentState.EntryAction(this);
           
        } 
    }
    


    public UnitsManager Unit
    {
        get { return _unit; }
        set { _unit = value; }
    }

    public Vector3 GetUnitPosition(){

        return Unit.UnitAgent.Position;
    }

     public State CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }


}

   