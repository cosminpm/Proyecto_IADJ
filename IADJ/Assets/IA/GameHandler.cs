using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Global;

// Manejador de combate
public class GameHandler : MonoBehaviour
{

    // Grid
    private GridMap _grid;

    // Lista de NPC del juego
    private List<NPC> _listNpcs;

    // Waypoint Manager
    [SerializeField] public WaypointManager waypointManager;
   


    void Start(){
        InitializeNPCS();
 
    }

    // Comprobamos en cada frame si hay algún equipo capturando alguna base, para restar puntos al equipo contrario.
    void Update(){

        bool blueCapturing = false;
        bool redCapturing = false;
        foreach( var n in _listNpcs)
        {
            // Si hay algún npc capturando la base enemiga, se resta puntos al equipo contrario.
            if( n.IsCapturing() )
            {
                if (n.GetUnitTeam() == (int) GlobalAttributes.Team.Red)
                    redCapturing = true;
                else    
                    blueCapturing = true;
            }

        }

        if (!redCapturing){
            waypointManager.NotCapturing(GlobalAttributes.Team.Red);
        }

        if (!blueCapturing){
            waypointManager.NotCapturing(GlobalAttributes.Team.Blue);
        }


    }








    private void InitializeNPCS(){
        GameObject[] blueNPCS = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL);
        GameObject[] redNPCS = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO);
        _listNpcs = new List<NPC>();

        foreach (var npc in blueNPCS){
             npc.GetComponent<NPC>().GameManager = this;
            _listNpcs.Add(npc.GetComponent<NPC>());
        }

        foreach (var npc in redNPCS){
             npc.GetComponent<NPC>().GameManager = this;
            _listNpcs.Add(npc.GetComponent<NPC>());
        }

    }



    
}

   