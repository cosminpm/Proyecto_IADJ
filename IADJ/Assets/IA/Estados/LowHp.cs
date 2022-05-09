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


        Face face = npc.GetComponent<Face>();
        if (face == null)
        {
            npc.gameObject.AddComponent<Face>();
            npc.gameObject.GetComponent<Face>().NewTarget(zoneBase);
            npc.Unit.UnitAgent.UpdateListSteering();
        }
        else
        {
            npc.GetComponent<Face>().enabled = true;
            npc.gameObject.GetComponent<Face>().NewTarget(zoneBase);
        }
    }

    public override void ExitAction(NPC npc)
    {
        movement = false;

        // npc.GetComponent<Seek>().enabled = false;
        // npc.GetComponent<Face>().enabled = false;
        npc.Unit.UnitAgent.UpdateListSteering();

        _targetNPC = null;
        healer = null;
        npc.pathFinding.ClearPath();
    }


    // TODO: En este estado, el NPC deber� buscar la zona de curaci�n m�s 
    // cercana y huir hacia all�.
    public override void Action(NPC npc, NPC obj)
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

                    npc.gameObject.GetComponent<Face>().NewTarget(a.Unit.UnitAgent);
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
                    
                    npc.gameObject.GetComponent<Face>().NewPositionTarget(posBase);
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
        if (IsDead(npc))
            return;

        // if (npc.stateManager.HealthPointReached(posBase, npc, healer, medicFound))
        //     return;

        if ( npc.stateManager.HealthPointReached(npc, npc.pathFinding.IsEndPath()))
            return;
    }


    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }


    // ESTO ESPRA DEBUG ELIMINAR
    public void CreateInvisible(){

        GameObject prediccionGO = new GameObject("AuxPursueIA");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = true;
        invisible.Position = posBase;
        invisible.InteriorRadius = 1.5f;
        invisible.ArrivalRadius = 1.0f;
        zoneBase = invisible;
    }
}
