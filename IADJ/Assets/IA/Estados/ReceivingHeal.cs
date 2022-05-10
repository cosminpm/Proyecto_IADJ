using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class ReceivingHeal : State
{
     void Awake(){
     //   stateImage = Resources.Load<Sprite>("Estados/sword");
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = false;
        _targetNPC = null;
    }

    public override void ExitAction(NPC npc)
    {
        _targetNPC = null;
    }

    // TODO: En este estado, el NPC deber� buscar la zona de curaci�n m�s 
    // cercana y huir hacia all�.
    public override void Action(NPC npc, NPC obj)
    {

        if ( npc.IsBase())
        {
            Debug.Log("Estoy en mi base as");
            npc.Unit.CurrentHealthPoints+=15;
        } else {
             Debug.Log("NO Estoy en mi base as");
        }

    }

    public override void CheckState(NPC npc)
    {
        if (IsDead(npc))
            return;
        if (npc.stateManager.HealingFinished(npc))
            return;
    }

    public override void Execute(NPC npc)
    {
        Action(npc, _targetNPC);
        CheckState(npc);
    }
}