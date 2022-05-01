using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

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
        costsTerrains = new Dictionary<TypeTerrains, float>();
        costsTerrains.Add(TypeTerrains.Path, 1.0f);
        costsTerrains.Add(TypeTerrains.Field, 0.9f);
        costsTerrains.Add(TypeTerrains.Forest, 0.2f);
        costsTerrains.Add(TypeTerrains.River, 0f);
        costsTerrains.Add(TypeTerrains.Cliff, 0f);
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

        HealthPointsMax = 225;
        HealthPointsMin = 75;
        CurrentHealthPoints = 225;
        AttackPoints = 25;
        // TODO: El rango se hace a ojo con el modo debug
        AttackRange = 10;
        AttackSpeed = 5;
        AttackAccuracy = 0.8f;
        CriticRate = 0.1f;
        VisionDistance = 5f; 
        TypeUnit = TypeUnits.Soldier;
    }

    /*public void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            // Velocidad
            Handles.color = Color.red;
            Handles.DrawLine(this.UnitAgent.Position, this.UnitAgent.Position.normalized + VisionDistance * OrientationToVector(this.UnitAgent.Orientation), 3);
        }
    }
    private Vector3 OrientationToVector(float _orientation)
    {
        Vector3 aux = new Vector3(Mathf.Sin(_orientation * Mathf.Deg2Rad), 0, Mathf.Cos(_orientation * Mathf.Deg2Rad));
        return aux.normalized;
    }*/
}

   

   