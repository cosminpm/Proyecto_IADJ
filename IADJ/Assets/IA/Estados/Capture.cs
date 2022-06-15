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


    public override void Action(NPC npc, NPC obj){
        
        
        if (!_capturing){

             if(npc.pathFinding.IsEndPath()){

                _capturing = true;
                if (movement){
                    movement = false;
                    npc.pathFinding.ClearPath();
                }
            //    npc.GameManager.waypointManager.Capturing(npc);

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

        if (!npc.IsTotalWar() && npc.stateManager.IsLowHP(npc))
            return;

        // Si somos un tanque y encontramos a un aliado que necesite 
        // de nuestra ayuda, vamos hacia él.
        if ( !npc.IsTotalWar() && npc.stateManager.BackupNeeded()){

          
            return;
        }

        // // Si hay enemigos en nuestra base, vamos hacia allá.
        // if (npc.stateManager.EnemiesInBase(npc))
        //     return;

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