using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Attack : State
{


    // TODO : SE PASA EL NPC COMO PARAMETRO XD??

    // Variable para indicar el tiempo (en frames)
    // que tardar� el NPC en lanzar su pr�ximo ataque.
    private int _cooldwnTime = 0;

    void Awake(){
     //   stateImage = Resources.Load<Sprite>("Estados/sword");
        stateImage.enabled = false;
    }

    // Cambiamos los pr�metros de entrada al estado.
    // P.E. antes de atacar, el NPC tiene que detenerse.
    public override void EntryAction(NPC npc){
        Debug.Log("Atacando ");
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


        // LO de abajo es mejor HACERLO EN EL ENTRY NO? QUE PASA EN EL CASO QUE AHAYA MAS DE UN ENEMIGO AL QUE ATACAR EN M,IU RANGO DE VISION ==============?
    
        Face f = npc.GetComponent<Face>();
        if (f == null)
        {
            npc.gameObject.AddComponent<Face>();
            npc.Unit.UnitAgent.UpdateListSteering();
            npc.gameObject.GetComponent<Face>().NewTarget(_targetNPC.Unit.UnitAgent);
        }
        else
        {
            npc.gameObject.GetComponent<Face>().NewTarget(_targetNPC.Unit.UnitAgent);
        }
 

        Vector3 direction = _targetNPC.GetUnitPosition() - npc.GetUnitPosition();
        // Distancia que hay entre el agente y el target
        float distance = direction.magnitude;

        // TODO: FALTA EL CONO XD

        // Si esta dentro de nuestro rango de ataque, atacamos
        if ( !_targetNPC.IsCurrentStateDead() && npc.Unit.AttackRange >= distance){


            // me dejo de mover
            if ( movement){
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
                _cooldwnTime+=npc.Unit.AttackSpeed;

            npc.GUI.UpdateBarAction(_cooldwnTime);
        } 
        
        else if ( !_targetNPC.IsCurrentStateDead())  {      // Tenemos que perseguir el objetivo, si se puede

            if ( !movement)
            {
                npc.pathFinding.CalculatePath(_targetNPC.Unit.UnitAgent.Position);
                movement = true;
            }

        }
    }

    public override void CheckState(NPC npc){
        // Comprobamos si el NPC debe salir del estado. 
        if (IsDead(npc))
            return;
        if (npc.stateManager.EnemiesInBase(npc))
            return;
        if (npc.stateManager.IsLowHP(npc))
            return;
        if (EnemyFound(npc))
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
