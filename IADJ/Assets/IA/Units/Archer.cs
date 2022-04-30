using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Archer : UnitsManager
{

       // TODO: para debug
     [SerializeField] protected bool _drawGizmos;

    void Start(){
        UnitAgent = GetComponent<AgentNPC>();
        SetMovementStats();
    }

    public Archer(){
        //base();

        SetUnitStats();
        AddCostsTerrain();
        AddUnitDamage();  

    }

    protected override void AddCostsTerrain(){
        costsTerrains = new Dictionary<TypeTerrains, float>();
        costsTerrains.Add(TypeTerrains.Path, 1.0f);
        costsTerrains.Add(TypeTerrains.Field, 0.9f);
        costsTerrains.Add(TypeTerrains.Forest, 0.2f);
        costsTerrains.Add(TypeTerrains.River, 0f);
        costsTerrains.Add(TypeTerrains.Cliff, 0f);
    }

    protected override void AddUnitDamage(){
        mapUnitDamage = new Dictionary<TypeUnits, float>();
        mapUnitDamage.Add(TypeUnits.Soldier, 1.5f);
        mapUnitDamage.Add(TypeUnits.Archer, 1f);
        mapUnitDamage.Add(TypeUnits.Tank, 0.5f);
    }

    protected override void SetMovementStats(){
        // Velocidad
        UnitAgent.Speed = 6;
        UnitAgent.MaxSpeed = 12;
        UnitAgent.MaxRotation = 30;
        UnitAgent.MaxAcceleration = 2.5f;
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
        VisionDistance = 5f; 
        TypeUnit = TypeUnits.Archer;
    }


     public void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            // Velocidad
            Handles.color = Color.red;
            Handles.DrawLine(UnitAgent.Position, UnitAgent.Position.normalized+VisionDistance* OrientationToVector(UnitAgent.Orientation), 1f);
        }
    }

    private Vector3 OrientationToVector(float _orientation)
    {
        
        Vector3 aux = new Vector3(Mathf.Sin(_orientation * Mathf.Deg2Rad), 0, Mathf.Cos(_orientation * Mathf.Deg2Rad));
        return aux.normalized;
    }




   


}

   