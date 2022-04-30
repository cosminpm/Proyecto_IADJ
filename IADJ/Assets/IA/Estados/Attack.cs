using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Attack : State
{


    // TODO : SE PASA EL NPC COMO PARAMETRO XD??

    private bool _attacking;

    public override void EntryAction(NPC npc){
        movement = false;
        _attacking = false;
    }

    public override void ExitAction(NPC npc){

        // Tenemos que limpiar el path
       // _targetNPC = null;
    }

    public override void Action(NPC npc, NPC _targetNPC){

        if ( _targetNPC == null)
        {
            Debug.Log("El tarjet es null");

        }

         if ( npc == null)
        {
            Debug.Log("Sor null e l atacoasda");
            
        }
        Vector3 direction = _targetNPC.GetUnitPosition() - npc.GetUnitPosition();
        // Distancia que hay entre el agente y el target
        float distance = direction.magnitude;

        // FALTA EL CONO XD

        // Si esta dentro de nuestro rango de ataque, atacamos
        if ( npc.Unit.AttackRange >= distance){

            movement = false;   
            float dmg = CombatHandler.CalculateDamage(npc, _targetNPC);
            
        } else {        // Tenemos que perseguir el objetivo, si se puede

            // Como nos hace falta cosmin para esto.. vamos a utilizar steerings ( es decir vectores de posicion en vez de casillas del grid)

            npc.gameObject.AddComponent<Seek>();
            npc.gameObject.GetComponent<Seek>().Target = _targetNPC.gameObject.GetComponent<AgentNPC>();

        }

      
    }

    public override void CheckState(NPC npc){

    }

    public override void Execute(NPC npc){
        Action(npc, _targetNPC);
        CheckState(npc);
    }

   


}

   