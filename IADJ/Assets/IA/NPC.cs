using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class NPC : MonoBehaviour
{


    // Tipo de unidad
    private UnitsManager _unit;
    
    // Controlador de estados
    public StateManager stateManager;

    // Movimiento del personaje
    private PathFinding pathFinding;

    // GUI NPC
    public GUIManager GUI;


    // Manejador de combate
    void Awake(){
        _unit = GetComponent<UnitsManager>();
        stateManager = GetComponent<StateManager>();
        GUI = GetComponent<GUIManager>();
        GUI.Initialize();
        stateManager.Initialize(this);
    }

    void Start(){
        
    }

    void Update(){

        // Si el NPC está en un estado, ejecuta la acción correspondiente
        stateManager.Execute(this); 

    }

    public UnitsManager Unit
    {
        get { return _unit; }
        set { _unit = value; }
    }

    public Vector3 GetUnitPosition(){
        return Unit.UnitAgent.Position;
    }

}

   