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
        npc.pathFinding.ClearPath();
        _targetNPC = null;
    }


    public override void Action(NPC npc){
        
        
        if (!_capturing){

             if(npc.pathFinding.IsEndPath()){

                _capturing = true;
                if (movement){
                    movement = false;
                    npc.pathFinding.ClearPath();
                }

            } else {

                // calculo el camino a la base enemiga
                if (!movement){
                    npc.pathFinding.CalculatePath(npc.GameManager.waypointManager.GetEnemyZonePosition(npc));
                    movement = true;
                } 
            }

        }
    }


    public override void CheckState(NPC npc){

        // Si estoy muerto, cambio al estado a muerto
        if  (npc.stateManager.IsDead())
             return;

        if (!npc.IsTotalWar() && npc.stateManager.IsLowHP())
            return;

        // Si somos un tanque y encontramos a un aliado que necesite 
        // de nuestra ayuda, vamos hacia él.
        if ( !npc.IsTotalWar() && npc.stateManager.BackupNeeded()){
            return;
        }

        // Si hay algún al que atacar, cambio de estado a MeleeAttack
        if (npc.stateManager.EnemyFound()){
            _captureTime = 1200;
            return;
        }

    }

    public override void Execute(NPC npc){
        Action(npc);
        CheckState(npc);
    }
}