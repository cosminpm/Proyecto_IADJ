using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Attack : State
{


    // Variable para indicar el tiempo (en frames)
    // que tardar� el NPC en lanzar su pr�ximo ataque.
    private int _cooldwnTime = 0;

    void Awake(){
        stateImage.enabled = false;
    }

    // Cambiamos los prÁmetros de entrada al estado.
    // P.E. antes de atacar, el NPC tiene que detenerse.
    public override void EntryAction(NPC npc){
        movement = false;
    }

    public override void ExitAction(NPC npc){
        _cooldwnTime = 0;
        npc.GUI.UpdateBarAction(_cooldwnTime);
        _targetNPC = null;
        npc.pathFinding.ClearPath();
    }

    public override void Action(NPC npc){

 
        if ( !_targetNPC.IsCurrentStateDead() )
        {
            Vector3 direction = _targetNPC.GetUnitPosition() - npc.GetUnitPosition();
            // Distancia que hay entre el agente y el target
            float distance = direction.magnitude;

            // Si esta dentro de nuestro rango de ataque, atacamos
            if (npc.Unit.AttackRange >= distance)
            {

                // me dejo de mover
                if (movement)
                {
                    movement = false;
                    npc.pathFinding.ClearPath();
                }

                if (_cooldwnTime >= 360)
                {
                    float dmg = CombatHandler.CalculateDamage(npc, _targetNPC);
                    _targetNPC.Unit.CurrentHealthPoints -= dmg;
                    _cooldwnTime = 0;
                }
                else
                    _cooldwnTime += npc.Unit.AttackSpeed;

                npc.GUI.UpdateBarAction(_cooldwnTime);
            }
            else 
            {
                npc.pathFinding.CalculatePath(_targetNPC.Unit.UnitAgent.Position);
                movement = true;
            }
        }
    }

    public override void CheckState(NPC npc){
        // Comprobamos si el NPC debe salir del estado. 
        if (npc.stateManager.IsDead())
            return;

        if (npc.stateManager.IsLowHP())
            return;

        if (npc.stateManager.EnemyFound())
            return;
        
;
    }

    public override void Execute(NPC npc){
        Action(npc);
        CheckState(npc);
    }

    public void SetObjetiveNPC(NPC npc){
        ObjetiveNPC = npc;
    }
}
