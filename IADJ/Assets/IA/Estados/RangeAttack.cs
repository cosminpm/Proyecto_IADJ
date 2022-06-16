using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Global;

public class RangeAttack : State
{

    private bool _attacking;
    // TODO : SE PASA EL NPC COMO PARAMETRO XD??

    // Variable para indicar el tiempo (en frames)
    // que tardar� el NPC en lanzar su pr�ximo ataque.
    private int _cooldwnTime = 0;
    private int _threshold = 15;
    private bool _arriveAllie;
    
    void Awake(){
        stateImage.enabled = false;
    }

    // Cambiamos los pr�metros de entrada al estado.
    // P.E. antes de atacar, el NPC tiene que detenerse.
    public override void EntryAction(NPC npc){
        movement = false;

        _attacking = true;
        _arriveAllie = false;
        
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

            // Lo de abajo se tiene Que hacer cuando huye 

            Vector3 direction = _targetNPC.GetUnitPosition() - npc.GetUnitPosition();
            // Distancia que hay entre el agente y el target
            float distance = direction.magnitude;

            // Obtenemos el tipo de terreno en el que estamos
            Cell cell = npc.pathFinding.WorldToMap(npc.GetUnitPosition());
            GridMap.TipoTerreno terrain = cell.GetTipoTerreno();

            float distanciaAliado = Mathf.Infinity;
            float distanciaBosque = Mathf.Infinity;

            // Obtenemos el aliado más cercano dentro de 
            // nuetro rango de vision
            NPC targetAllie = npc.FindClosestAllie();

            // Buscamos el bosque mas cercano
           // Vector3 posBosque = npc.pathFinding.GetClosestCellTypeInRange(npc.Unit.UnitAgent.Position, npc.GetUnitVisionDistance(), GridMap.TipoTerreno.Bosque);

            // Si estamos justo dentro de nuestro rango de ataque, atacamos
        
            if (_attacking){
                // me dejo de mover
                if (movement){
                    movement = false;
                    npc.pathFinding.ClearPath();
                }

                if (_cooldwnTime >= 360) {

                    float dmg = CombatHandler.CalculateDamage(npc, _targetNPC);
                    _targetNPC.Unit.CurrentHealthPoints -= dmg;
                    _cooldwnTime = 0;
                    _attacking = false;
                }

                else { 
                    _cooldwnTime += npc.Unit.AttackSpeed;
                }

                npc.GUI.UpdateBarAction(_cooldwnTime);
            } else if (distance > npc.GetUnitAttackRange()){
                _attacking = true;
            }
            
            // Comprobamos si el target se ha acercado a mi posición

            else if ( distance-_threshold <= npc.GetUnitAttackRange() && !movement ) {


                // Si hemos encontrado al aliado, calculamos la distancia
                // hacia el
                // if (targetAllie != null)
                // {
                //     Vector3 direccionAliado = _targetNPC.GetUnitPosition() - npc.GetUnitPosition();
                //     distanciaAliado = direccionAliado.magnitude;
                //     Debug.Log("Voy al ladiado weandk");

                //     npc.pathFinding.CalculatePath(targetAllie.Unit.UnitAgent.Position);
                //     _arriveAllie = false;
                //     movement = true;
                // } else {

                    npc.pathFinding.CalculatePath(npc.GameManager.waypointManager.GetBasePosition(npc));
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
    }

    public override void Execute(NPC npc){
        Action(npc);
        CheckState(npc);
    }

    public void SetObjetiveNPC(NPC npc){
        ObjetiveNPC = npc;
    }
}
