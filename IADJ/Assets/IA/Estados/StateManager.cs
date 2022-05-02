using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager: MonoBehaviour
{

    // Estados posibles del NPC   
    public Capture stateCapture;
    public Dead stateDead;
    public LowHp stateLowHp;
    public Attack stateAttack;
    public Healing stateHealing;


    // Estado Actual del npc
    private State _currentState;

    void Awake(){

        stateCapture = this.gameObject.GetComponent<Capture>();
       // stateDead = this.gameObject.GetComponent<Dead>();
       // stateLowHp = this.gameObject.GetComponent<LowHp>();
        stateAttack = this.gameObject.GetComponent<Attack>();
       // stateHealing = this.gameObject.GetComponent<Healing>();
        _currentState = stateCapture;
        _currentState.stateImage.enabled = true;

    }

    public void Initialize(NPC npc){
      //  ChangeState(stateCapture, npc);
    }

    public void Execute(NPC npc){
        if ( _currentState != null)
            _currentState.Execute(npc); 
    }

    // Función para cambiar el estado del NPC
    public void ChangeState(State newState, NPC npc){

       
        if ( _currentState != null && _currentState != newState)
        {
             Debug.Log("Estoy cambiandaosd asd");
            _currentState.ExitAction(npc);
        }

        if (_currentState != newState) {
            npc.GUI.UpdateStateImagen(_currentState,newState);
            _currentState = newState;
            _currentState.EntryAction(npc);
        } 
    }

    public State CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
       
    
}
