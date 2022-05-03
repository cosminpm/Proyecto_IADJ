using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHp : State
{

    // PARA DEBUG!!
    private AgentInvisible zoneBase;

    // Posicion base (MAS CORRECTO EN EL GAME MANAGER)
    private Vector3 posBase = new Vector3(25,0,-5);

    private bool medicFound = false;

    private NPC healer;

    void Awake(){
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = true;
        _targetNPC = null;
        Debug.Log("Me queda poca vida :(");
        CreateInvisible();
        // Por defecto me voy a mi base
        Seek arr = npc.GetComponent<Seek>(); 
        if ( arr == null){
            npc.gameObject.AddComponent<Seek>();
            npc.gameObject.GetComponent<Seek>().NewTarget(zoneBase);
            npc.Unit.UnitAgent.UpdateListSteering();

        } else {
            npc.GetComponent<Seek>().enabled = true;
            npc.gameObject.GetComponent<Seek>().NewTarget(zoneBase);
        }
    }

    public override void ExitAction(NPC npc)
    {
        movement = false;

        npc.GetComponent<Seek>().enabled = false;
        npc.Unit.UnitAgent.UpdateListSteering();
        _targetNPC = null;
        healer = null;
    }


    // TODO: En este estado, el NPC deber� buscar la zona de curaci�n m�s 
    // cercana y huir hacia all�.
    public override void Action(NPC npc, NPC obj)
    {

        if (medicFound && !npc.isInVisionRange(healer) )
        {
            npc.gameObject.GetComponent<Seek>().NewTarget(zoneBase);
            medicFound = false;
        } else { 
         

            // Encuentro mis aliados cercanos
            List<NPC> allies = npc.FindNearbyAllies();
            // Hay un healear ?
            foreach ( var a in allies)
            {
                // La unidad aliada es un healear?
                if ((int) a.GetUnitType() == 3)
                {   
                    npc.gameObject.GetComponent<Seek>().NewTarget(a.Unit.UnitAgent);
                    medicFound = true;
                    healer = a;
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
        if (npc.stateManager.HealthPointReached(posBase, npc, healer, medicFound))
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
        invisible.InteriorRadius = 3.5f;
        invisible.ArrivalRadius = 4.0f;
        zoneBase = invisible;
    }
}
