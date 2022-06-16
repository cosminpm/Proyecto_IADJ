using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Grid;

public class Healer : UnitsManager
{

     // TODO: para debug
     [SerializeField] protected bool _drawGizmos;

    void Start(){
        UnitAgent = GetComponent<AgentNPC>();
        SetMovementStats();
        _maxSpeed = UnitAgent.MaxSpeed;
    }

    public Healer(){
        //base();
        SetUnitStats();
        AddCostsTerrain();
        ActivateNormalMode();
    }
    protected override void AddCostsTerrain(){
        costsTerrains = new Dictionary<GridMap.TipoTerreno, float>();
        costsTerrains.Add(GridMap.TipoTerreno.Camino, 0.9f);
        costsTerrains.Add(GridMap.TipoTerreno.Pradera, 0.1f);
        costsTerrains.Add(GridMap.TipoTerreno.Bosque, 0.1f);
        costsTerrains.Add(GridMap.TipoTerreno.Rio,  Mathf.Infinity);
        costsTerrains.Add(GridMap.TipoTerreno.Acantilado, 0.9f);

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
        UnitAgent.Speed = 3;
        UnitAgent.MaxSpeed = 5;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 2.5f;
        UnitAgent.MaxAngularAcceleartion = 35;
        UnitAgent.InteriorRadius = 3.5f;
        UnitAgent.ArrivalRadius = 4.0f;
    }

    protected override void SetUnitStats(){
        
        HealthPointsMax = 225;
        HealthPointsMin = 75;
        CurrentHealthPoints = HealthPointsMax;
        AttackPoints = -25;
        AttackRange = 12;
        AttackAccuracy = 0.8f;
        CriticRate = 0.1f;
        VisionDistance = 23; 
        TypeUnit = TypeUnits.Healer;
        Influence = 0.1f;
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
      AttackRange = 35;
      CriticRate = 0.5f;
      HealthPointsMin = 35;
      UnitAgent.MaxSpeed = 45f;
    }

    // Funcion para activar el modo Defensive.
    // En modo defensivo, los arqueros se situaran 
    // mas lejos y huiran antes, pero acertaran 
    // menos golpes criticos.

    public override void ActivateDefensiveMode()
    {
        Mode = Modes.Defensive;
        AttackRange = 75;
        CriticRate = 0.1f;
        HealthPointsMin = 75;
    }

    // Funcion para activar el modo Normal.
    public override void ActivateNormalMode()
    {
        Mode = Modes.Normal;
        AttackRange = 15f;
        CriticRate = 0.3f;
        HealthPointsMin = 75;
    }


    
}

   