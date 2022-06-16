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

    // Lista de NPC Rojos del juego
    public List<NPC> _listNpcsRed;

    public List<NPC> _listNpcsBlue;

    // GUI JUEGO
    public GUIManagerGlobal gui;

    // Waypoint Manager
    [SerializeField] public WaypointManager waypointManager;

    // Units Manager
    public UnitsManager unitsManager;

    // Mode Total War Activatw
    private bool _totalWar = false;



    void Start(){
        InitializeNPCS();
 
        gui = GetComponent<GUIManagerGlobal>();
        foreach( var npc in _listNpcsRed)
            npc.GameManager = this;

        foreach( var npc in _listNpcsBlue)
            npc.GameManager = this;

    }

    // Comprobamos en cada frame si hay algún equipo capturando alguna base, para restar puntos al equipo contrario.
    void Update(){

        bool blueCapturing = false;
        bool redCapturing = false;

        if (!CheckVictory()) {


            // Hay que comprobar si el porcentaje de captura de alguna es preocupante.. Pasar a estado defensivo
            bool defensiveRed = waypointManager.GetPercentageCaptureBlue() >= 0.5f;
            bool defensiveBlue = waypointManager.GetPercentageCaptureRed() >= 0.5f;

            bool offensiveRed = waypointManager.GetPercentageCaptureRed() >= 0.1f;
            bool offensiveBlue = waypointManager.GetPercentageCaptureBlue() >= 0.1f;

            foreach (var npc in _listNpcsRed)
            {                
                
                if (defensiveRed){
                    npc.DefendBase();
                } else {
                    if(!_totalWar)
                        npc.ActivateNormalMode();
                }

                if (offensiveRed){
                    npc.AttackEnemyBase();
                } else {

                    if (!_totalWar)
                        npc.ActivateNormalMode();
                }

                // Si hay algún npc capturando la base enemiga, se resta puntos al equipo contrario.
                if (npc.IsCapturing()){
                        redCapturing = true;
                }

                
            }

            foreach (var npc in _listNpcsBlue)
            {

                if (defensiveBlue){
                    npc.DefendBase();
                } else {
                    if (!_totalWar)
                        npc.ActivateNormalMode();
                }

                if (offensiveRed){
                    npc.AttackEnemyBase();
                } else {
                    if (!_totalWar)
                        npc.ActivateNormalMode();
                }



                // Si hay algún npc capturando la base enemiga, se resta puntos al equipo contrario.
                if (npc.IsCapturing())
                    blueCapturing = true;
            }

            if (!redCapturing)
            {
            //    Debug.Log("No hay nadie del equipo rojo capturando");
                waypointManager.NotCapturing(GlobalAttributes.Team.Red);
            }

            if (!blueCapturing)
            {
              //  Debug.Log("No hay nadie del equipo azul capturando");
                waypointManager.NotCapturing(GlobalAttributes.Team.Blue);
            }
        }
    }

    // Funcion para comprobar si la partida ha acabado.
    private bool CheckVictory() {
        // if (waypointManager.blueEnemyBase.winningPercentage >= 1)
        // {
        //     Debug.Log("Victoria para el equipo rojo!");
        //     return true;
        // }

        // else if (waypointManager.redEnemyBase.winningPercentage >= 1) 
        // {
        //     Debug.Log("Victoria para el equipo azul!");
        //     return true;
        // }
        return false;
    }

    public void ButtonModeOfensive(){
        Debug.Log("Modo Ofensivo");
          
        foreach (var n in _listNpcsRed)
            n.Unit.ActivateOffensiveMode();

        foreach (var n in _listNpcsRed)
            n.Unit.ActivateOffensiveMode();

    }

    public void ButtonModeDefensive(){
        Debug.Log("Modo defensivo");

        foreach (var n in _listNpcsRed)
            n.Unit.ActivateDefensiveMode();
        foreach (var n in _listNpcsRed)
            n.Unit.ActivateDefensiveMode();
    }

     public void ButtonModeNormal(){
        Debug.Log("Modo Normal");

        foreach (var n in _listNpcsRed)
            n.Unit.ActivateNormalMode();
        foreach (var n in _listNpcsRed)
            n.Unit.ActivateNormalMode();
    }


    public void ButtonModeTotalWar(){
        Debug.Log("Total war activado");
        foreach (var n in _listNpcsBlue)
            n.ActivateTotalWarMode();

        foreach (var n in _listNpcsRed)
            n.ActivateTotalWarMode();

        _totalWar = true;
    }


    private void InitializeNPCS(){
        GameObject[] blueNPCS = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL);
        GameObject[] redNPCS = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO);
        _listNpcsRed = new List<NPC>();
        _listNpcsBlue = new List<NPC>();

        foreach (var npc in redNPCS){
            if (npc.GetComponent<NPC>() != null)
             _listNpcsRed.Add(npc.GetComponent<NPC>());
        }

        foreach (var npc in blueNPCS){
            if (npc.GetComponent<NPC>() != null)
            _listNpcsBlue.Add(npc.GetComponent<NPC>());
        }
    }

    // Busca los npcs más cercanos al npc pasado como parametro
    public List<NPC> FindNearbyAllies(NPC npc){

        // Lista de los npc aliados
        List<NPC> listAllies = new List<NPC>();

        List<NPC> list = new List<NPC>();

        // Dependiendo del equipo que sea, obtengo la lista de mi equipo
        switch ( (int) npc.GetUnitTeam() )
        {
            case (int) GlobalAttributes.Team.Blue:
               listAllies = _listNpcsBlue;

                break;
            case (int) GlobalAttributes.Team.Red:
                listAllies = _listNpcsRed;
                break;

            default:
                break;
        }

        // Si están en mi rango de visión...
        foreach(var n in listAllies){
            if ( npc.IsInVisionRange(n) && !n.IsCurrentStateDead() && n.name != npc.name)
                list.Add(n);
        }
        return list;
    }

    // Busca los npcs más cercanos al npc pasado como parametro
    public List<NPC> FindNearbyEnemies(NPC npc){

        // Lista de los npc aliados
        List<NPC> listEnemies = new List<NPC>();

        List<NPC> list = new List<NPC>();

        // Dependiendo del equipo que sea, obtengo la lista de mi equipo
        switch ( (int) npc.GetUnitTeam() )
        {
            case (int) GlobalAttributes.Team.Blue:
               listEnemies = _listNpcsRed;

                break;
            case (int) GlobalAttributes.Team.Red:
                listEnemies = _listNpcsBlue;
                break;
            default:
                break;
        }

        // Si están en mi rango de visión...
        foreach(var n in listEnemies){
            if ( npc.IsInVisionRange(n) && !n.IsCurrentStateDead() && !n.IsCurrentStateReceivingHealing())
                list.Add(n);
        }
        return list;
    }

    public bool GameIsTotalWar(){
        return _totalWar;
    }
}

   