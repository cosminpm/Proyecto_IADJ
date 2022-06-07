using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Global;

public class RangeAttack : State
{


    // TODO : SE PASA EL NPC COMO PARAMETRO XD??

    // Variable para indicar el tiempo (en frames)
    // que tardar� el NPC en lanzar su pr�ximo ataque.
    private int _cooldwnTime = 0;
    private int threshold;
    
    void Awake(){
     //   stateImage = Resources.Load<Sprite>("Estados/sword");
        stateImage.enabled = false;
    }

    // Cambiamos los pr�metros de entrada al estado.
    // P.E. antes de atacar, el NPC tiene que detenerse.
    public override void EntryAction(NPC npc){
        movement = false;

        Face f = npc.GetComponent<Face>();

        //Pathfinding c = npc.GetComponent<Pathfinding>();

        // if ( f != null )
        //     npc.GetComponent<Face>().enabled = true;

        // if ( s != null )
        //     npc.GetComponent<Seek>().enabled = true;
        //if ( c != null)
       //     npc.GetComponent<Pathfinding>().enabled = true;
    }

    public override void ExitAction(NPC npc){
        // Tenemos que limpiar el path
        // _targetNPC = null;
        
        _cooldwnTime = 0;
        npc.GUI.UpdateBarAction(_cooldwnTime);
        // npc.GetComponent<Seek>().enabled = false;
        // npc.GetComponent<Face>().enabled = false;
     //   npc.GetComponent<Pathfinding>().enabled = false;
        npc.Unit.UnitAgent.UpdateListSteering();
        _targetNPC = null;
        npc.pathFinding.ClearPath();
    }

    public override void Action(NPC npc, NPC _targetNPC){


        if ( !_targetNPC.IsCurrentStateDead() )
        {
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
            Vector3 posBosque = npc.pathFinding.GetClosestCellTypeInRange(npc.Unit.UnitAgent.Position, npc.GetUnitVisionDistance(), GridMap.TipoTerreno.Bosque);

            // Si estamos justo dentro de nuestro rango de ataque, atacamos
            if (npc.Unit.AttackRange == distance ||
                ((npc.Unit.AttackRange >= distance && 
                GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().WorldToMap(npc.GetUnitPosition()).GetTipoTerreno() == GridMap.TipoTerreno.Bosque)))
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

            // Si estamos a un rango de ataque menor y hay un bosque o un aliado cerca, vamos hacia allí
            else if (distance < npc.Unit.AttackRange && (targetAllie != null || posBosque != Vector3.zero))
            {
                // Si hemos encontrado al aliado, calculamos la distancia
                // hacia el
                if (targetAllie != null)
                {
                    Vector3 direccionAliado = _targetNPC.GetUnitPosition() - npc.GetUnitPosition();
                    distanciaAliado = direccionAliado.magnitude;
                }

                // Si se ha encontrado un bosque, calculamos la distancia a el
                if (posBosque != Vector3.zero)
                {
                    Vector3 dirBosque = posBosque - npc.Unit.UnitAgent.Position;
                    distanciaBosque = dirBosque.magnitude;
                }

                // Comprobamos cual es la distancia mas cercana y mandamos el arquero hacia alli
                if (distanciaAliado < distanciaBosque)
                {
                    npc.pathFinding.CalculatePath(targetAllie.Unit.UnitAgent.Position);
                    movement = true;
                }

                else if (distanciaBosque != Mathf.Infinity)
                {
                    npc.pathFinding.CalculatePath(posBosque);
                    movement = true;
                }
            }

            // Si estamos demasiado cerca y no hay refugio, nos desplazamos a la base aliada
            // hasta tener una distancia de ataque suficiente
            else if (distance < npc.Unit.AttackRange && !(targetAllie != null || posBosque != Vector3.zero))
            {
                npc.pathFinding.CalculatePath(npc.GameManager.waypointManager.GetBasePosition(npc));
                movement = true;
            }

            // Si el enemigo esta fuera del rango de ataque, nos desplazamos hasta alcanzarlo
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

        if (npc.stateManager.IsLowHP(npc))
            return;

        if (EnemyFound(npc))
            return;

        if (npc.stateManager.EnemiesInBase(npc))
            return;
    }

    public override void Execute(NPC npc){
        Action(npc, _targetNPC);
        CheckState(npc);
    }

    public void SetObjetiveNPC(NPC npc){
        ObjetiveNPC = npc;
    }
}
