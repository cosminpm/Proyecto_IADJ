using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHp : State
{

    // PARA DEBUG!!
    private AgentInvisible zoneBase;

    // Posicion base (MAS CORRECTO EN EL GAME MANAGER)
    private Vector3 posBase = new Vector3(10,0,-5);

    private bool medicFound;

    private NPC healer;

    void Awake(){
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {

        movement = false;
        _targetNPC = null;
        medicFound = false;

        npc.ActivateSpeedBonus();


    }

    public override void ExitAction(NPC npc)
    {
        movement = false;
        _targetNPC = null;
        healer = null;
        npc.pathFinding.ClearPath();
        npc.DeactivateSpeedBonus();

    }


    // TODO: En este estado, el NPC deber� buscar la zona de curaci�n m�s 
    // cercana y huir hacia all�.
    public override void Action(NPC npc)
    {

        // // Si no está en rango de visión, lo intento perseguir ??
        if ( medicFound)
        {
            // Tengo que comprobar si el médico sigue vivo o sigue en mi rango de visión
            if ( healer.IsCurrentStateDead() || !npc.IsInVisionRange(healer)){
                medicFound = false;
                npc.pathFinding.ClearPath();
                movement = false;
            }

        }
        else { 
       //  Encuentro mis aliados cercanos
            List<NPC> allies = npc.GameManager.FindNearbyAllies(npc);
            // Hay un healear dentro de mi rango de viision ? ?
            foreach ( var a in allies)
            {
                
                if ((int) a.GetUnitType() == 3)
                {   
                    // He encontrado  un medico en mi rango de visión
                    npc.pathFinding.CalculatePath(a.GetUnitPosition());
                    medicFound = true;
                    movement = true;
                    healer = a;
                } 

            }

            // Me voy a la base
            if ( !medicFound)
            {
                if (!movement){
                
                    npc.pathFinding.CalculatePath(npc.GameManager.waypointManager.GetBasePosition(npc));
                    movement = true;
                }
            }

        }
    }

    public override void CheckState(NPC npc)
    {
        // TODO: Aqu� hay que comprobar si ha llegado a una zona de
        // curaci�n. Si ese es el caso habr� que pasar a un estado Healing.
        //
        // Tambi�n hay que comprobar si el NPC est� muerto, ya que alguien
        // puede matarlo por el camino.
        if (npc.stateManager.IsDead())
            return;
        
        if (npc.stateManager.TotalWar())
            return;
        // if (npc.stateManager.HealthPointReached(posBase, npc, healer, medicFound))
        //     return;
        if ( npc.stateManager.HealthPointReached(npc.pathFinding.IsEndPath()))
            return;
    }


    public override void Execute(NPC npc)
    {
        Action(npc);
        CheckState(npc);
    }

}
