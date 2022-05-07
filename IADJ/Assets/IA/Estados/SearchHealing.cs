using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class SearchHealing : State
{
    
     void Awake(){
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = true;
        Wander w = npc.GetComponent<Wander>();

        // if ( w != null )
        //     npc.GetComponent<Wander>().enabled = true;
        // else{
        //      npc.gameObject.AddComponent<Wander>();
        // }
    }

    public override void ExitAction(NPC npc)
    {
        movement = false;
        _targetNPC = null;
        Debug.Log("Encontré a alguien a quien curar :)");
       // npc.GetComponent<Wander>().enabled  = false;
    }

    public override void Action(NPC npc, NPC obj)
    {
        
    }

    public override void CheckState(NPC npc)
    {
        // Cuando ccambio ? de estado
        if ( IsDead(npc))
            return;

        // Hay que cambiar de estado cuadno 
        // Se encuentre un aliado (campo de visión) y ese personaje esté en estado lowHP.
        if (npc.stateManager.AllieHealthReached(npc))
            return;
    }

    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }
}