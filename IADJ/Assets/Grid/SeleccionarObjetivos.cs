using System;
using System.Collections.Generic;
using Global;
using Grid;
using Pathfinding;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/*
 * L. Daniel Hernández. 2018. Copyleft
 * 
 * Una propuesta para dar órdenes a un grupo de agentes sin formación.
 * 
 * Recursos:
 * Los rayos de Cámara: https://docs.unity3d.com/es/current/Manual/CameraRays.html
 * "Percepción" mediante Physics.Raycast: https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
 * SendMessage to external functions: https://www.youtube.com/watch?v=4j-lh3C_w1Q
 * 
 * */

public class SeleccionarObjetivos : MonoBehaviour
{
    private List<GameObject> _listNpCs = new List<GameObject>();
    private Material _matRojo, _matBlanco;
    [SerializeField] private GameObject personajeInvisible;
    
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectNPCs();
        SelectAllUnits();
        SendOrder();
    }

    public List<GameObject> getListNPCs()
    {
        return new List<GameObject>(_listNpCs);
    }

    private void SelectNPCs()
    {
        // Comprobamos si se hace click en algun punto del escenario.
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider != null && CompareTagTeam(hitInfo))
                {
                    
                    GameObject npc = GameObject.Find(hitInfo.collider.gameObject.name);
                    Debug.Log(npc.name);
                    if (_listNpCs.Contains(npc))
                    {
                        _listNpCs.Remove(npc);
                        
                        npc.transform.Find(GlobalAttributes.NAME_BOLA_AMARILLA).gameObject.SetActive(false);
                        if (_listNpCs.Count > 0)
                            GameObject.Find(GlobalAttributes.NAME_MINICAMERA).GetComponent<CameraMinimap>().SetTransform(_listNpCs[_listNpCs.Count-1].transform);
                    }
                    else
                    {
                        _listNpCs.Add(npc);
                        GameObject.Find(GlobalAttributes.NAME_MINICAMERA).GetComponent<CameraMinimap>().SetTransform(npc.transform);
                        npc.transform.Find(GlobalAttributes.NAME_BOLA_AMARILLA).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    
    
    
    private void SendOrder()
    {
        if (Input.GetMouseButtonUp(1))
        {
            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Si se esta chocando algo...
                if (hitInfo.collider != null)
                {
                    Agent agent;
                    if (GlobalAttributes.CheckIfItIsFloor(hitInfo.collider.tag))
                    {
                        Vector3 position = hitInfo.point;
                        Vector3 center = GetComponent<GridMap>().WorldToMap(position).GetCenter();
                        GameObject ai = CreateInvisibleAgent(center);
                        agent = ai.GetComponent<Agent>();
                    }
                    // En caso de que no se esté clickando nada, el método termina
                    else
                    {
                        return;
                    }

                    foreach (var npc in _listNpCs)
                    {
                        SendNewTarget(npc, agent);
                        Cell finsihCell = GetComponent<GridMap>().CheckIfCellClicked(true);
                        npc.GetComponent<ControlPathFindingWithSteering>().SendOrder(finsihCell);
                        
                    }
                }
            }
        }
    }
    
    
    //
    // if (Input.GetKeyUp(KeyCode.Alpha2))
    // {
    //     Cell finishCell = gridMap.CheckIfCellClicked(true);
    //     SendOrder(finishCell);
    // }
        
        
    // TODO: SE TIENE QUE ELIMINAR
    private void SendNewTarget(GameObject npc, Agent agent)
    {
        {
            // Steerings Basicos
            if (npc.GetComponent<Seek>())
                npc.GetComponent<Seek>().NewTarget(agent);

            if (npc.GetComponent<Flee>())
                npc.GetComponent<Flee>().NewTarget(agent);

            if (npc.GetComponent<VelocityMatching>())
                npc.GetComponent<VelocityMatching>().NewTarget(agent);

            if (npc.GetComponent<Arrive>())
                npc.GetComponent<Arrive>().NewTarget(agent);

            if (npc.GetComponent<Align>())
                npc.GetComponent<Align>().NewTarget(agent);

            // Steerings Delegados
            if (npc.GetComponent<Pursue>())
                npc.GetComponent<Pursue>().NewTarget(agent);

            if (npc.GetComponent<Evade>())
                npc.GetComponent<Evade>().NewTarget(agent);

            if (npc.GetComponent<Face>())
                npc.GetComponent<Face>().NewTarget(agent);

            if (npc.GetComponent<PathFollowingCell>())
                npc.GetComponent<PathFollowingCell>().NewTarget(agent);
        }
    }

    private bool CompareTagTeam(RaycastHit hitInfo)
    {
        return hitInfo.collider.transform.CompareTag("BaseRoja") || hitInfo.collider.transform.CompareTag("BaseAzul");
    }
    
    private void SelectAllUnits()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            _listNpCs = new List<GameObject>(GameObject.FindGameObjectsWithTag("NPC"));
            foreach (var npc in _listNpCs)
            {
                npc.GetComponent<MeshRenderer>().material = _matRojo;
            }
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            foreach (var npc in _listNpCs)
            {
                SendNewTarget(npc, null);
                npc.GetComponent<MeshRenderer>().material = _matBlanco;
            }

            _listNpCs = new List<GameObject>();
        }
    }

    public virtual GameObject CreateInvisibleAgent(Vector3 positionSpawn)
    {
        string nombreAI = GlobalAttributes.NAME_AGENTE_INVISIBLE;
        GameObject ai = GameObject.Find(nombreAI);
        ai.transform.position = positionSpawn;
        return ai;
    }

    public void SetInteriorAndExteriorRadiusAgentInvisible(float interiorRadius, float exteriorRadius)
    {
        GameObject.Find(GlobalAttributes.NAME_AGENTE_INVISIBLE).GetComponent<AgentInvisible>().InteriorAngle = interiorRadius;
        GameObject.Find(GlobalAttributes.NAME_AGENTE_INVISIBLE).GetComponent<AgentInvisible>().ExteriorAngle = exteriorRadius;
    }
    
    private void OnDrawGizmos()
    {
        // if (_listNpCs!= null && _listNpCs.Count > 0)
        //     DrawAllPlayersSelected();
    }
}