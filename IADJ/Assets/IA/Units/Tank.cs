using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Tank : UnitsManager
{

   
  void Start(){
        UnitAgent = GetComponent<AgentNPC>();
        SetMovementStats();
    }

    public Tank(){
        //base();

        SetUnitStats();
        AddCostsTerrain();

    }

    protected override void AddCostsTerrain(){
        costsTerrains = new Dictionary<TypeTerrains, float>();
        costsTerrains.Add(TypeTerrains.Path, 1.0f);
        costsTerrains.Add(TypeTerrains.Field, 0.9f);
        costsTerrains.Add(TypeTerrains.Forest, 0.2f);
        costsTerrains.Add(TypeTerrains.River, 0f);
        costsTerrains.Add(TypeTerrains.Cliff, 0f);
    }

    protected override void SetMovementStats(){
        // Velocidad
        UnitAgent.Speed = 2;
        UnitAgent.MaxSpeed = 4;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 1;
        UnitAgent.MaxAngularAcceleartion = 35;
    }

    protected override void SetUnitStats(){

        HealthPointsMax = 150;
        HealthPointsMin = 50;
        CurrentHealthPoints = 150;
        AttackPoints = 10;
        // TODO: El rango se hace a ojo con el modo debug
        AttackRange = 50;
        AttackSpeed = 0.5f;
        AttackAccuracy = 0.9f;
        VisionDistance = 70; 
         TypeUnit = TypeUnits.Tank;
    }



}

   