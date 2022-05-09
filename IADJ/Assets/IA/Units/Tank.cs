using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Grid;

public class Tank : UnitsManager
{
    // TODO: para debug
    [SerializeField] protected bool _drawGizmos;
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
        UnitAgent.Speed = 1;
        UnitAgent.MaxSpeed = 2;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 1;
        UnitAgent.MaxAngularAcceleartion = 80;
        UnitAgent.MaxRotation = 80;
        UnitAgent.InteriorRadius = 5.0f;
        UnitAgent.ArrivalRadius = 5.5f;
    }

    protected override void SetUnitStats(){

        // Modificamos las estadisticas del
        // NPC en funci�n del modo en el 
        // que se encuentre.

        // En el caso del tanque, se modificara la 
        // vida maxima y los puntos de ataque.
        if (Mode == Modes.Normal || Mode == Modes.TotalWar)
        {
            HealthPointsMax = 500;
            AttackPoints = 60;
        }

        if (Mode == Modes.Offensive)
        {
            HealthPointsMax = 400;
            AttackPoints = 25;
        }

        // En modo defensivo, la funcion del tanque sera
        // la de recibir golpes en defensa de sus compa�eros.
        // A cambio de su vida extra, perdera ataque.
        if (Mode == Modes.Defensive)
        {
            HealthPointsMin = 600;
            AttackPoints = 5;
        }

        HealthPointsMin = 100;
        CurrentHealthPoints = HealthPointsMax;
        AttackRange = 5;
        AttackSpeed = 3;
        AttackAccuracy = 0.9f;
        CriticRate = 0.1f;
        VisionDistance = 10; 
        TypeUnit = TypeUnits.Tank;
    }

    
}
