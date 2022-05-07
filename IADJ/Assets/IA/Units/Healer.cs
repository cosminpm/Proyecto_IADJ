using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

public class Healer : UnitsManager
{

     // TODO: para debug
     [SerializeField] protected bool _drawGizmos;

    void Start(){
        UnitAgent = GetComponent<AgentNPC>();
        SetMovementStats();
    }

    public Healer(){
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
        UnitAgent.MaxAcceleration = 2.5f;
        UnitAgent.MaxAngularAcceleartion = 35;
        UnitAgent.InteriorRadius = 3.5f;
        UnitAgent.ArrivalRadius = 4.0f;
    }

    protected override void SetUnitStats(){

        // Modificamos las estadisticas del
        // NPC en funci�n del modo en el 
        // que se encuentre.

        // Los curanderos en modo ofensivo
        // se mantienen igual que en modo
        // normal.
        if (Mode == Modes.Normal || Mode == Modes.Offensive || Mode == Modes.TotalWar)
        {
            AttackSpeed = 4;
        }

        // Los curanderos en modo defensivo
        // curaran mas rapido de lo normal.
        if (Mode == Modes.Defensive)
        {
            AttackSpeed = 7;
        }

        HealthPointsMax = 225;
        HealthPointsMin = 75;
        CurrentHealthPoints = HealthPointsMax;
        AttackPoints = -25;
        AttackRange = 9;
        AttackAccuracy = 0.8f;
        CriticRate = 0.1f;
        VisionDistance = 10; 
        TypeUnit = TypeUnits.Healer;
    }
}

   