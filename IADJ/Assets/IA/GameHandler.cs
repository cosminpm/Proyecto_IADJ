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

            // Comprobamos el cambio de estado.
            IsChangingMode();

            foreach (var n in _listNpcsRed)
            {                
                // Si hay algún npc capturando la base enemiga, se resta puntos al equipo contrario.
                if (n.IsCapturing())
                    redCapturing = true;
            }

            foreach (var n in _listNpcsBlue)
            {
                // Si hay algún npc capturando la base enemiga, se resta puntos al equipo contrario.
                if (n.IsCapturing())
                    blueCapturing = true;
            }

            if (!redCapturing)
            {
                Debug.Log("No hay nadie del equipo rojo capturando");
                waypointManager.NotCapturing(GlobalAttributes.Team.Red);
            }

            if (!blueCapturing)
            {
                Debug.Log("No hay nadie del equipo azul capturando");
                waypointManager.NotCapturing(GlobalAttributes.Team.Blue);
            }
        }
    }

    // Funcion para comprobar si la partida ha acabado.
    private bool CheckVictory() {
        if (waypointManager.blueEnemyBase.winningPercentage >= 1)
        {
            Debug.Log("Victoria para el equipo rojo!");
            return true;
        }

        else if (waypointManager.redEnemyBase.winningPercentage >= 1) 
        {
            Debug.Log("Victoria para el equipo azul!");
            return true;
        }
        return false;
    }

    // Función para comprobar si se ha realizado un cambio de modo.
    private bool IsChangingMode() {

        // Si pulsamos la tecla "T", se activa el modo TotalWar.
        if (Input.GetKey(KeyCode.T))
        {
            foreach (var n in _listNpcsBlue)
                n.Unit.ActivateTotalWar();

            foreach (var n in _listNpcsRed)
                n.Unit.ActivateTotalWar();

            return true;
        }

        // Si pulsamos la tecla "R", seleccionamos al equipo rojo.
        else if (Input.GetKey(KeyCode.R)) 
        {
            // Si se pulsa "A", cambiamos el modo del equipo rojo a "modo ofensivo".
            if (Input.GetKey(KeyCode.A))
            {
                foreach (var n in _listNpcsRed)
                    n.Unit.ActivateOffensiveMode();

                return true;
            }

            // Si se pulsa "D", cambiamos el modo del equipo rojo a "modo defensivo".
            else if (Input.GetKey(KeyCode.D))
            {
                foreach (var n in _listNpcsRed)
                    n.Unit.ActivateDefensiveMode();

                return true;
            }

            // Si se pulsa "N", cambiamos el modo del equipo rojo a "modo normal".
            else if (Input.GetKey(KeyCode.N))
            {
                foreach (var n in _listNpcsRed)
                    n.Unit.ActivateNormalMode();

                return true;
            }
        }

        // Si pulsamos la tecla "B", seleccionamos al equipo azul.
        else if (Input.GetKey(KeyCode.B))
        {
            // Si se pulsa "A", cambiamos el modo del equipo azul a "modo ofensivo".
            if (Input.GetKey(KeyCode.A))
            {
                foreach (var n in _listNpcsBlue)
                    n.Unit.ActivateOffensiveMode();

                return true;
            }

            // Si se pulsa "D", cambiamos el modo del equipo azul a "modo defensivo".
            else if (Input.GetKey(KeyCode.D))
            {
                foreach (var n in _listNpcsBlue)
                    n.Unit.ActivateDefensiveMode();

                return true;
            }

            // Si se pulsa "N", cambiamos el modo del equipo azul a "modo normal".
            else if (Input.GetKey(KeyCode.N))
            {
                foreach (var n in _listNpcsBlue)
                    n.Unit.ActivateNormalMode();

                return true;
            }
        }

        return false;
    }

    private void InitializeNPCS(){
        GameObject[] blueNPCS = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_AZUL);
        GameObject[] redNPCS = GameObject.FindGameObjectsWithTag(GlobalAttributes.TAG_EQUIPO_ROJO);
        _listNpcsRed = new List<NPC>();
        _listNpcsBlue = new List<NPC>();

        //TODO:  // Se crea un NPC MÁS.. puede ser que un terreno tenga un script NPC.. Probar con el mapa definitivo
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
            if ( npc.IsInVisionRange(n) && !n.IsCurrentStateDead())
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
            if ( npc.IsInVisionRange(n) && !n.IsCurrentStateDead())
                list.Add(n);
        }
        return list;
    }
}

   