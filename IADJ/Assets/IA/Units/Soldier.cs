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
        _maxSpeed = UnitAgent.MaxSpeed;

    }

    public Soldier(){
        //base();
        SetUnitStats();
        AddCostsTerrain();
        ActivateNormalMode();
    }
    protected override void AddCostsTerrain(){
        costsTerrains = new Dictionary<GridMap.TipoTerreno, float>();
        costsTerrains.Add(GridMap.TipoTerreno.Camino, 0.1f);
        costsTerrains.Add(GridMap.TipoTerreno.Pradera, 0.5f);
        costsTerrains.Add(GridMap.TipoTerreno.Bosque, 0.6f);
        costsTerrains.Add(GridMap.TipoTerreno.Rio, Mathf.Infinity);
        costsTerrains.Add(GridMap.TipoTerreno.Acantilado, 1.5f);

        if ( UnitTeam == Team.Red){
            costsTerrains.Add(GridMap.TipoTerreno.BaseRoja, 1f);
            costsTerrains.Add(GridMap.TipoTerreno.BaseAzul, 1f);
        } else {
            costsTerrains.Add(GridMap.TipoTerreno.BaseRoja, 1f);
             costsTerrains.Add(GridMap.TipoTerreno.BaseAzul, 1f);
        } 
    }

    protected override void SetMovementStats(){
        // Velocidad
        UnitAgent.Speed = 2;
        UnitAgent.MaxSpeed = 4;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 2.5f;
        UnitAgent.MaxAngularAcceleartion = 35;
        UnitAgent.InteriorRadius = 1f;
        UnitAgent.ArrivalRadius = 1f;
    }

    protected override void SetUnitStats(){


        HealthPointsMax = 500;
        CurrentHealthPoints = HealthPointsMax;
        AttackRange = 5;
        AttackSpeed = 6;
        AttackPoints = 20;
        AttackAccuracy = 0.8f;
        CriticRate = 0.1f;
        VisionDistance = 10; 
        TypeUnit = TypeUnits.Soldier;
        Influence = 2f;
    }


     // Funcion para activar el modo TotalWar.
    public override void ActivateTotalWar() {
        Mode = Modes.TotalWar;

    }

    // Funcion para activar el modo Offensive.
    // Los arqueros en modo ofensivo tender�n a
    // situarse algo mas cerca y acertar 
    // mas golpes criticos.

    public override void ActivateOffensiveMode()
    {
      Mode = Modes.Offensive;
      CriticRate = 0.5f;
      HealthPointsMin = 35;
    }

    // Funcion para activar el modo Defensive.
    // En modo defensivo, los arqueros se situaran 
    // mas lejos y huiran antes, pero acertaran 
    // menos golpes criticos.

    public override void ActivateDefensiveMode()
    {
        Mode = Modes.Defensive;
        AttackPoints = 20;
        CriticRate = 0.1f;
        HealthPointsMin = 75;
    }

    // Funcion para activar el modo Normal.
    public override void ActivateNormalMode()
    {
        Mode = Modes.Normal;
        AttackRange = 5f;
        CriticRate = 0.1f;
        HealthPointsMin = 75;
    }



    
}

   

   