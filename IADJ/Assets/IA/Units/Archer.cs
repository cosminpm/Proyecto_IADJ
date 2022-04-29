using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Archer : UnitsManager
{

    [SerializeField] protected AgentNPC agentNPC;

    public Archer(){
        //base();

        HealthPointsMax = 150;
        HealthPointsMin = 50;
        CurrentHealthPoints = 150;
        AttackPoints = 10;
        // TODO: El rango se hace a ojo con el modo debug
        AttackRange = 50;
        AttackSpeed = 0.5f;
        AttackAccuracy = 0.9f;
        VisionDistance = 70; 

        if ( agentNPC == null){
            Debug.Log("JEJEJEJ");
        }

        Inicializar();
        

    }

    public void Inicializar(){
            // Velocidad de movimiento
        agentNPC.Speed = 10;
    }
   


}

   