using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : State
{

    private int _cooldwnTime = 0;

    void Awake(){
     //   stateImage = Resources.Load<Sprite>("Estados/sword");
        stateImage.enabled = false;
    }

    // Start is called before the first frame update
    public override void EntryAction(NPC npc)
    {
        movement = false;
        _targetNPC = null;
        Debug.Log("Me queda poca vida :(");
    }

    public override void ExitAction(NPC npc)
    {
        _targetNPC = null;
    }

    // TODO: En este estado, el NPC deber� buscar la zona de curaci�n m�s 
    // cercana y huir hacia all�.
    public override void Action(NPC npc, NPC targetNPC)
    {
        Debug.Log("Estoy curando :)");

        Vector3 direction = targetNPC.GetUnitPosition() - npc.GetUnitPosition();
        // Distancia que hay entre el agente y el target
        float distance = direction.magnitude;

        // Si esta dentro de nuestro rango de ataque, atacamos
        if (npc.Unit.AttackRange >= distance)
        {
            movement = false;
            if (_cooldwnTime >= 360)
            {
                //  Debug.Log(_targetNPC.Unit.TypeUnit + " tiene " + _targetNPC.Unit.CurrentHealthPoints + " puntos de vida");
                float dmg = npc.GetAttackPoints();
                targetNPC.Unit.CurrentHealthPoints -= dmg;
                _cooldwnTime = 0;
            }
            else
                _cooldwnTime += npc.Unit.AttackSpeed;
        }
        npc.GUI.UpdateBarAction(_cooldwnTime);

    }
    public override void CheckState(NPC npc)
    {
        // if (IsDead(npc) || npc.stateManager.CureFinished(npc))
        //     return;
        if (IsDead(npc))
            return;

        if(npc.stateManager.CureFinished(npc, _targetNPC))
            return;
    }

    public override void Execute(NPC npc)
    {
        Action(npc, npc.stateManager.CurrentState._targetNPC);
        CheckState(npc);
    }
}
