using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Grid;

public class Soldier : UnitsManager
{
     // TODO: para debug
     [SerializeField] protected bool _drawGizmos;

    void Start(){
        UnitAgent = GetComponent<AgentNPC>();
        SetMovementStats();
    }

    public Soldier(){
        //base();
        SetUnitStats();
        AddCostsTerrain();
    }
    protected override void AddCostsTerrain(){
        costsTerrains = new Dictionary<GridMap.TipoTerreno, float>();
        costsTerrains.Add(GridMap.TipoTerreno.Camino, 1.0f);
        costsTerrains.Add(GridMap.TipoTerreno.Pradera, 0.9f);
        costsTerrains.Add(GridMap.TipoTerreno.Bosque, 0.2f);
        costsTerrains.Add(GridMap.TipoTerreno.Rio, 0f);
        costsTerrains.Add(GridMap.TipoTerreno.Acantilado, 0f);

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

        if (Mode == Modes.Normal || Mode == Modes.TotalWar) {
            HealthPointsMax = 225;
            HealthPointsMin = 75;
            AttackPoints = 55;
        }

        // El soldado en modo ofensivo atacara mas
        // contundentemente al enemigo y huira mas tarde,
        // pero perdera vida maxima. 
        if (Mode == Modes.Offensive)
        {
            HealthPointsMax = 200;
            HealthPointsMin = 50;
            AttackPoints = 35;
        }


        // El soldado en modo defensivo ganara puntos de
        // vida maximos, pero huira antes y atacara con 
        // menos fuerza.
        if (Mode == Modes.Defensive)
        {
            HealthPointsMax = 250;
            HealthPointsMin = 125;
            AttackPoints = 15;
        }

        CurrentHealthPoints = HealthPointsMax;
        AttackRange = 5;
        AttackSpeed = 4;
        AttackAccuracy = 0.8f;
        CriticRate = 0.1f;
        VisionDistance = 10; 
        TypeUnit = TypeUnits.Soldier;
    }

    
}

   

   