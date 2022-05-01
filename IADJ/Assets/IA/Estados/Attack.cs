using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Attack : State
{


    // TODO : SE PASA EL NPC COMO PARAMETRO XD??

    private bool _attacking;

    // Variable para indicar el tiempo (en frames)
    // que tardar� el NPC en lanzar su pr�ximo ataque.
    private int _cooldwnTime = 360;

    // Cambiamos los pr�metros de entrada al estado.
    // P.E. antes de atacar, el NPC tiene que detenerse.
    public override void EntryAction(NPC npc){
        movement = false;
        _attacking = false;
        Debug.Log("AH, un men! Me pego >:|");
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

        // TODO: FALTA EL CONO XD

        // Si esta dentro de nuestro rango de ataque, atacamos
        if ( npc.Unit.AttackRange >= distance){
            movement = false;

            if (_cooldwnTime <= 0)
            {
                Debug.Log(_targetNPC.Unit.TypeUnit + " tiene " + _targetNPC.Unit.CurrentHealthPoints + " puntos de vida");
                float dmg = CombatHandler.CalculateDamage(npc, _targetNPC);
                _targetNPC.Unit.CurrentHealthPoints -= dmg;
                _cooldwnTime = 360;
            }
            else
                _cooldwnTime-=npc.Unit.AttackSpeed;
        } 
        
        else if (npc.Unit.VisionDistance >= distance) {      // Tenemos que perseguir el objetivo, si se puede

            // Como nos hace falta cosmin para esto.. vamos a utilizar steerings ( es decir vectores de posicion en vez de casillas del grid)
            npc.gameObject.AddComponent<Seek>();
            npc.gameObject.GetComponent<Seek>().Target = _targetNPC.gameObject.GetComponent<AgentNPC>();
        }
    }

    public override void CheckState(NPC npc){
        // Comprobamos si el NPC debe salir del estado. 
        // Esto lo har� cuando est� a poca vida, haya 
        // muerto o ya no encuentre a un enemigo.
        if(NeedHeal(npc) || IsDead(npc) || EnemyFound(npc))
            return;
    }

    public override void Execute(NPC npc){
        Action(npc, _targetNPC);
        CheckState(npc);
    }
}

   