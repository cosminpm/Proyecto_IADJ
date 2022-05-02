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
    public LowHp stateLowHp;
    public Attack stateAttack;
    public Healing stateHealing;

    // Movimiento del personaje
    private PathFinding pathFinding;

    // Estado actual del NPC
    private State _currentState;

    // GUI NPC
    public GUIManager GUI;

    // Manejador de combate
    void Awake(){
        _unit = GetComponent<UnitsManager>();
        // RARO LO DE ABAJO
        stateCapture = this.gameObject.AddComponent<Capture>();
        stateDead = this.gameObject.AddComponent<Dead>();
        stateLowHp = this.gameObject.AddComponent<LowHp>();
        stateAttack = this.gameObject.AddComponent<Attack>();
        stateHealing = this.gameObject.AddComponent<Healing>();

        GUI = GetComponent<GUIManager>();
        GUI.Initialize();
        ChangeState(stateCapture);
    }

    void Start(){
        
    }

    void Update(){

        // Si el NPC está en un estado, ejecuta la acción correspondiente
        if ( _currentState != null){ 
            _currentState.Execute(this);  
        } 


    }

    // Función para cambiar el estado del NPC
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

   