using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : State
{
    // PARA DEBUG!!
    private AgentInvisible zoneBase;

    // Posicion base (MAS CORRECTO EN EL GAME MANAGER)
    private Vector3 posBase = new Vector3(25, 0, -5);

    private bool medicFound = false;

    private NPC healer;

    void Awake()
    {
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = true;
        _targetNPC = null;

        CreateInvisible();

        // Si atacan nuestra base, me voy a defenderla.
        Seek seek = npc.GetComponent<Seek>();
        if (seek == null)
        {
            npc.gameObject.AddComponent<Seek>();
            npc.gameObject.GetComponent<Seek>().NewTarget(zoneBase);
            npc.Unit.UnitAgent.UpdateListSteering();

        }
        else
        {
            npc.GetComponent<Seek>().enabled = true;
            npc.gameObject.GetComponent<Seek>().NewTarget(zoneBase);
        }

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

        npc.GetComponent<Seek>().enabled = false;
        npc.GetComponent<Face>().enabled = false;
        npc.Unit.UnitAgent.UpdateListSteering();
        _targetNPC = null;
        healer = null;
    }

    public override void Action(NPC npc, NPC obj)
    {
    }

    public override void CheckState(NPC npc)
    {
        if (IsDead(npc))
            return;


        // TODO: Esto se tiene que hacer con el grid.
        Vector3 direction = posBase - npc.GetUnitPosition();
        float distance = direction.magnitude; 
        // Si ha llegado a la base y hay enemigos 
        // cerca, el NPC ataca.
        if (direction.magnitude == 0 && EnemyFound(npc))
            return;
    }


    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }


    // ESTO ESPRA DEBUG ELIMINAR
    public void CreateInvisible()
    {

        GameObject prediccionGO = new GameObject("AuxPursueIA");
        AgentInvisible invisible = prediccionGO.AddComponent<AgentInvisible>();
        prediccionGO.GetComponent<AgentInvisible>().DrawGizmos = true;
        invisible.Position = posBase;
        invisible.InteriorRadius = 3.5f;
        invisible.ArrivalRadius = 4.0f;
        zoneBase = invisible;
    }
}
