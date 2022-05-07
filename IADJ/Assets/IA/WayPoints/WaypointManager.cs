using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Global;
using Grid;

// Manejador de combate
public class WaypointManager : MonoBehaviour
{


    // Grid del mapa
    public GridMap grid;

    //Game manager
    public GameHandler gameManager;

    //GUI
    public GUIManagerGlobal GUI;


    [SerializeField] public Waypoint redBase;
    [SerializeField] public Waypoint blueBase; 
    [SerializeField] public Waypoint blueEnemyBase; 
    [SerializeField] public Waypoint redEnemyBase;  
    [SerializeField] public Waypoint patrolArea;  
    

    // Devuelve la base aliada
    public Waypoint GetBase(NPC npc){
        if ( npc.GetUnitTeam() == (int)GlobalAttributes.Team.Red)
            return redBase;
        else 
            return blueBase;
    }

    // Devuelve la zona de captura enemiga
    public Waypoint GetEnemyZone(NPC npc){
        if ( npc.GetUnitTeam() == (int)GlobalAttributes.Team.Red)
            return blueEnemyBase;
        else 
            return redEnemyBase;
    }

     // Devuelve la zona de captura aliada
    public Waypoint GetAllieZone(NPC npc){
        if ( npc.GetUnitTeam() == (int)GlobalAttributes.Team.Red)
            return redEnemyBase;
        else 
            return blueEnemyBase;
    }

    public Vector3 GetEnemyZonePosition(NPC npc){

        return GetRandomWaypointPosition(GetEnemyZone(npc));
    }

    public Vector3 GetBasePosition(NPC npc){

        return GetRandomWaypointPosition(GetBase(npc));
    }


    //Devuelve una posicion aleatoria dentro de un waypoint
    private Vector3 GetRandomWaypointPosition (Waypoint waypoint){
        int random = Random.Range(0, waypoint.waypointPos.Length);
        return waypoint.waypointPos[random].position;
    }

    public bool InsideWaypoint(NPC npc, Waypoint waypoint) {
        
        // Vector3 direction;
        // float distance;

        // foreach (var pos in waypoint.waypointPos) {
        //     direction =  npc.GetUnitPosition( ) - pos.position;
        //     distance  = direction.magnitude;

        //     if (distance <= waypoint.distanceMinWaypoint)
        //         return true;
        // }
        // return false;

        foreach (Transform pos in waypoint.waypointPos) {
     
            if (Vector3.Distance(npc.GetUnitPosition(), pos.position) <= waypoint.distanceMinWaypoint)
                return true;
        }
        return false;
    }


    public void Capturing(NPC npc){

        // Estoy capturando la base enemiga
        if (npc.GetUnitTeam() == (int) GlobalAttributes.Team.Red) 
        {
            redEnemyBase.winningPercentage+= 0.90f * Time.deltaTime;
            Debug.Log("Estoy dando a puntos a mi equioi");
        }  else {
            blueEnemyBase.winningPercentage+= 0.90f * Time.deltaTime;
            Debug.Log("Estoy dando a puntos a mi equioi ADASDASDS " );
        }

    }

    public void NotCapturing(GlobalAttributes.Team team){
        
        switch( team)
        {
            case GlobalAttributes.Team.Red:
                blueBase.winningPercentage -= 0.40f;
                if ( blueBase.winningPercentage <= 0)
                    blueBase.winningPercentage = 0;
                break;
            case GlobalAttributes.Team.Blue:
                redBase.winningPercentage -= 0.40f;
                if ( redBase.winningPercentage <= 0)
                    redBase.winningPercentage = 0;
                break;

            default:
                break;
        }
    }

    // public void NotCapturingRed(){
    //     blueBase.winningPercentage -= 0.40f
    //     if ( blueBase.winningPercentage <= 0)
    //         blueEnemyBase.winningPercentage = 0;
    // }

    public bool wonTeamBlue(){
        if ( blueEnemyBase.winningPercentage == 200)
            return true;
        else    
            return false;
    }

    public bool wonTeamRed(){
        if ( redEnemyBase.winningPercentage == 200)
            return true;
        else
            return false;
    }



  

}

   