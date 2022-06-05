using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Grid;


public class Archer : UnitsManager
{
     // TODO: para debug
     [SerializeField] protected bool _drawGizmos;

    void Start(){
        UnitAgent = GetComponent<AgentNPC>();
        SetMovementStats();
        _maxSpeed = UnitAgent.MaxSpeed;
    }

    public Archer(){
        //base();
        SetUnitStats();
        AddCostsTerrain();
    }

    protected override void AddCostsTerrain(){

        costsTerrains = new Dictionary<GridMap.TipoTerreno, float>();
        costsTerrains.Add(GridMap.TipoTerreno.Camino, 0.2f);
        costsTerrains.Add(GridMap.TipoTerreno.Pradera, 1f);
        costsTerrains.Add(GridMap.TipoTerreno.Bosque, 0.1f);
        costsTerrains.Add(GridMap.TipoTerreno.Rio, 0f);
        costsTerrains.Add(GridMap.TipoTerreno.Acantilado, 0.9f);

        if ( UnitTeam == Team.Red){
            costsTerrains.Add(GridMap.TipoTerreno.BaseRoja, 0.5f);
            costsTerrains.Add(GridMap.TipoTerreno.BaseAzul, 2f);
        } else {
            costsTerrains.Add(GridMap.TipoTerreno.BaseRoja, 2f);
             costsTerrains.Add(GridMap.TipoTerreno.BaseAzul, 0.5f);
        }
        
    }

    protected override void SetMovementStats(){
        // Velocidad
        UnitAgent.Speed = 2.5f;
        UnitAgent.MaxSpeed = 4.5f;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 2.5f;
        UnitAgent.MaxAngularAcceleartion = 35;
    }

    protected override void SetUnitStats(){

        // Modificamos las estadisticas del
        // NPC en funci�n del modo en el 
        // que se encuentre.

        // En el caso del arquero, se modificara el 
        // rango de ataque, la tasa de criticos y el momento en el que huyan.
        if (Mode == Modes.Normal || Mode == Modes.TotalWar)
        {
            AttackRange = 50;
            CriticRate = 0.3f;
            HealthPointsMin = 50;
        }

        // Los arqueros en modo ofensivo tender�n a
        // situarse algo mas cerca y acertar 
        // mas golpes criticos.
        if (Mode == Modes.Offensive)
        {
            AttackRange = 35;
            CriticRate = 0.5f;
            HealthPointsMin = 35;
        }

        // En modo defensivo, los arqueros se situaran 
        // mas lejos y huiran antes, pero acertaran 
        // menos golpes criticos.
        if (Mode == Modes.Defensive)
        {
            AttackRange = 75;
            CriticRate = 0.1f;
            HealthPointsMin = 75;
        }

        HealthPointsMax = 150;
        CurrentHealthPoints = HealthPointsMax;
        AttackPoints = 10;
        AttackSpeed = 3;
        AttackAccuracy = 0.75f;
        VisionDistance = 5f; 
        TypeUnit = TypeUnits.Archer;
    }


}

   