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
        _maxSpeed = UnitAgent.MaxSpeed;
    }

    public Tank(){
        //base();
        SetUnitStats();
        AddCostsTerrain();
        ActivateNormalMode();
    }

    protected override void AddCostsTerrain(){
        costsTerrains = new Dictionary<GridMap.TipoTerreno, float>();
        costsTerrains.Add(GridMap.TipoTerreno.Camino, 0.2f);
        costsTerrains.Add(GridMap.TipoTerreno.Pradera, 0.3f);
        costsTerrains.Add(GridMap.TipoTerreno.Bosque, 0.8f);
        costsTerrains.Add(GridMap.TipoTerreno.Rio, Mathf.Infinity);
        costsTerrains.Add(GridMap.TipoTerreno.Acantilado, 0.3f);

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
        UnitAgent.Speed = 1.5f;
        UnitAgent.MaxSpeed = 3.5f;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 1;
        UnitAgent.MaxAngularAcceleartion = 80;
        UnitAgent.MaxRotation = 80;
        UnitAgent.InteriorRadius = 5.0f;
        UnitAgent.ArrivalRadius = 5.5f;
    }

    protected override void SetUnitStats(){


        HealthPointsMax = 300;
        HealthPointsMin = 100;
        CurrentHealthPoints = HealthPointsMax;
        AttackRange = 3;
        AttackSpeed = 3;
        AttackPoints = 20;
        AttackAccuracy = 0.9f;
        CriticRate = 0.1f;
        VisionDistance = 13; 
        TypeUnit = TypeUnits.Tank;
        Influence = 3f;
    }


     // Funcion para activar el modo TotalWar.
    public override void ActivateTotalWar() {
        Mode = Modes.TotalWar;

    }

    // Funcion para activar el modo Offensive.
    // Los arqueros en modo ofensivo tender???n a
    // situarse algo mas cerca y acertar 
    // mas golpes criticos.

    public override void ActivateOffensiveMode()
    {
      Mode = Modes.Offensive;
      AttackRange = 8;
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
        AttackRange = 5;
        CriticRate = 0.1f;
        HealthPointsMin = 75;
    }

    // Funcion para activar el modo Normal.
    public override void ActivateNormalMode()
    {
        Mode = Modes.Normal;
        AttackRange = 5f;
        CriticRate = 0.3f;
        HealthPointsMin = 75;
    }


    
}
