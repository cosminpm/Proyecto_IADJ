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

    private int currentPatrollWaypoint;
    private int pathDir = 1;


    [SerializeField] public Waypoint redBase;
    [SerializeField] public Waypoint blueBase; 
    [SerializeField] public Waypoint blueEnemyBase; 
    [SerializeField] public Waypoint redEnemyBase;  
    [SerializeField] public Waypoint redPatrollArea;
    [SerializeField] public Waypoint bluePatrollArea;
    [SerializeField] public Waypoint redRoamingArea;
    [SerializeField] public Waypoint blueRoaminglArea;

    // TODO: Tener en cuenta que los objetos en el waypoint de las patrullas deben estar ordenados

    // Devuelve la base aliada
    public Waypoint GetBase(NPC npc){
        if ( npc.GetUnitTeam() == (int)GlobalAttributes.Team.Red)
            return redBase;
        else 
            return blueBase;
    }

    // Devuelve la base aliada
    public Waypoint GetPatrollZone(NPC npc)
    {
        if (npc.GetUnitTeam() == (int)GlobalAttributes.Team.Red)
            return redPatrollArea;
        else
            return bluePatrollArea;
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

    // Obtenemos el punto de patrullaje mas cercano al NPC.
    public Vector3 GetClosestPatrollWaypoint(NPC npc)
    {
        Vector3 direction;
        float distance;
        float minDistance = Mathf.Infinity;
        Transform auxWaypoint = null;
        int i = 0;

        foreach (var w in GetPatrollZone(npc).waypointPos)
        {
            direction = w.position - npc.GetUnitPosition();
            distance = direction.magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                auxWaypoint = w;
                currentPatrollWaypoint = i;
            }

            i++;
        }
        return auxWaypoint.position;
    }

    public Vector3 GetNextPatrollWaypoint(NPC npc)
    {
        if ((currentPatrollWaypoint + 1) == GetPatrollZone(npc).GetWaypointPosLenght() || currentPatrollWaypoint <= 0)
        {
            pathDir *= -1;
        }

        currentPatrollWaypoint = currentPatrollWaypoint + pathDir;
        return GetPatrollZone(npc).waypointPos[currentPatrollWaypoint].position;
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
            if (Vector3.Distance(npc.GetUnitPosition(), pos.position) < waypoint.distanceMinWaypoint)
                return true;
        }
        return false;
    }


    public void Capturing(NPC npc){

        // Estoy capturando la base enemiga
        if (npc.GetUnitTeam() == (int) GlobalAttributes.Team.Red) 
        {
            blueEnemyBase.winningPercentage += 0.001f;
        }  
        
        else 
        {
            redEnemyBase.winningPercentage += 0.001f;
        }

    }

    public void NotCapturing(GlobalAttributes.Team team){
        
        switch( team)
        {
            case GlobalAttributes.Team.Red:
                blueEnemyBase.winningPercentage -= 0.009f;
                if ( blueEnemyBase.winningPercentage <= 0)
                    blueEnemyBase.winningPercentage = 0;
                break;
            case GlobalAttributes.Team.Blue:
                redEnemyBase.winningPercentage -= 0.009f;
                if ( redEnemyBase.winningPercentage <= 0)
                    redEnemyBase.winningPercentage = 0;
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
        if ( blueEnemyBase.winningPercentage == 100)
            return true;
        else    
            return false;
    }

    public bool wonTeamRed(){
        if ( redEnemyBase.winningPercentage == 100)
            return true;
        else
            return false;
    }

    public float GetPercentageCaptureRed(){
        return blueEnemyBase.winningPercentage;
    }

    public float GetPercentageCaptureBlue(){
        return redEnemyBase.winningPercentage;
    }



  

}

   