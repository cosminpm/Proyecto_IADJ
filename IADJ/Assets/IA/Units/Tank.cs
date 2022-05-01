using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

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

        HealthPointsMax = 300;
        HealthPointsMin = 50;
        CurrentHealthPoints = 300;
        AttackPoints = 10;
        // TODO: El rango se hace a ojo con el modo debug
        AttackRange = 5;
        AttackSpeed = 1;
        AttackAccuracy = 0.9f;
        CriticRate = 0.1f;
        VisionDistance = 3; 
        TypeUnit = TypeUnits.Tank;
    }
    /*public void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            // Velocidad
            Handles.color = Color.red;
            Handles.DrawLine(UnitAgent.Position, UnitAgent.Position.normalized + VisionDistance * OrientationToVector(UnitAgent.Orientation), 1f);
        }
    }
    private Vector3 OrientationToVector(float _orientation)
    {
        Vector3 aux = new Vector3(Mathf.Sin(_orientation * Mathf.Deg2Rad), 0, Mathf.Cos(_orientation * Mathf.Deg2Rad));
        return aux.normalized;
    }*/

}
