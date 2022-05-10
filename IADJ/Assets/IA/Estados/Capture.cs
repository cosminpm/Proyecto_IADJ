using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Capture : State
{
    // Tiempo que tarda una unidad en capturar una zona.
    private int _captureTime = 1200;
    private bool _capturing;


    void Awake(){
        stateImage.enabled = true;
    }

    public override void EntryAction(NPC npc){
        movement = false;
        _capturing = false;
    }

    public override void ExitAction(NPC npc){
        _targetNPC = null;
    }


    public override void Action(NPC npc, NPC obj){
        
        
        if (npc.IsCapturing()){
            
             // me dejo de mover
            if (movement){
                movement = false;
                npc.pathFinding.ClearPath();
            }

            npc.GameManager.waypointManager.Capturing(npc);

        } else {

            // calculo el camino a la base enemiga
            if (!movement){
                npc.pathFinding.CalculatePath(npc.GameManager.waypointManager.GetEnemyZonePosition(npc));
                movement = true;
            } 

        }




        // if (_captureTime <= 0)
        // {
        //     Debug.Log("He marcado un punto! :D");
        //     _captureTime = 1200;
        // }
        // // TODO: Esto hay que cambiarlo. La captura se tiene que hacer si 
        // // el NPC está en la ZONA enemiga, no en un punto concreto. 
        // else if (npc.GetUnitPosition() == new Vector3(-115, 0, 100))
        //     _captureTime--;


    }


    public override void CheckState(NPC npc){

        // Si estoy muerto, cambio al estado a muerti
        if  (IsDead(npc) || npc.stateManager.EnemiesInBase(npc)){
             return;
        }

        // Si hay algún al que atacar, cambio de estado a MeleeAttack
        if (EnemyFound(npc)){
            _captureTime = 1200;
            return;
        }

       // npc.ChangeState(npc.stateCapture);
    }

    public override void Execute(NPC npc){
        Action(npc, _targetNPC);
        CheckState(npc);
    }
}