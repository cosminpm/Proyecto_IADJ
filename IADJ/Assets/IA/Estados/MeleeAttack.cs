using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : State
{


    // TODO : SE PASA EL NPC COMO PARAMETRO XD??

    private bool _attacking;

    public override void EntryAction(NPC npc){
        movement = false;
        _attacking = false;
    }

    public override void ExitAction(NPC npc){

        // Tenemos que limpiar el path
    }

    public override void Action(NPC npc){

      
    }

    public override void CheckState(NPC npc){

    }


    // Versión 1: Encontramos el enemigo más cercano. TODO: Se puede refactorizar ( Modular funciones)
    private NPC findClosestEnemy(NPC npc){

        GameObject[] enemys;

        // Dependiendo del equipo que sea, busco mis enemigos
        switch ((int) npc.Unit.UnitTeam)
        {
            case 0:
                enemys = GameObject.FindGameObjectsWithTag("TeamBlue");
                break;
            case 1:
                enemys = GameObject.FindGameObjectsWithTag("TeamRed");
                break;
            default:
                enemys = null;
                break;
        }


         // Obtenemos la dirección hacia el target
        Vector3 direction;
        // Distancia que hay entre el agente y el target
        float distance;

        float minDistance = Mathf.Infinity;

        NPC auxEnemy;

        // Recorro todos mis enemigos y me quedo con el más cercano.
        foreach ( var e in enemys)
        {
            direction = e.transform.position - npc.GetUnitPosition();
            distance = direction.magnitude;

            if ( distance < minDistance)
            {
                auxEnemy = e.GetComponent<NPC>();
                minDistance = distance;
            } 
        }

        // Compruebo si está dentro del rango de ataque

        if ( npc.Unit.AttackRange >= minDistance )
        {
            // atacamos
            Debug.Log("Atacamos");
        } else {

            // Perseguimos con un pursue
            Debug.Log("Perseguimos");
        }




        return npc;
    }


}

   