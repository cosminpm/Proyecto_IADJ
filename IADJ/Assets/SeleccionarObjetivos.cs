using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.PackageManager;
using UnityEngine;

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
    private List<GameObject> listNPCs = new List<GameObject>();
    private Material matRojo, matBlanco;
    [SerializeField] private GameObject personajeInvisible;
    private void Start()
    {
        matRojo = new Material(Shader.Find("Standard"));
        matRojo.color = Color.red;

        matBlanco = new Material(Shader.Find("Standard"));
        matBlanco.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        SelectNPCs();
        SendOrder();
    }
    
    public List<GameObject> GetListNPCS()
    {
        return new List<GameObject>(listNPCs);
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
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("NPC"))
                {
                    GameObject npc = GameObject.Find(hitInfo.collider.gameObject.name);
                    if (listNPCs.Contains(npc))
                    {
                        SendNewTarget(npc, null);
                        listNPCs.Remove(npc);
                        npc.GetComponent<MeshRenderer>().material = matBlanco;
                    }
                    else
                    {
                        listNPCs.Add(npc);
                        npc.GetComponent<MeshRenderer>().material = matRojo;
                    }
                }
            }
        }

        GetListNPCS();
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
                    if (hitInfo.collider.CompareTag("Terrain"))
                    {
                        Vector3 position = hitInfo.point;
                        GameObject ai = CreateInvisibleAgent(position);
                        agent = ai.GetComponent<Agent>();
                    }
                  
                    else if (hitInfo.collider.CompareTag("Prota") || hitInfo.collider.CompareTag("NPC"))
                    {
                        agent = hitInfo.transform.gameObject.GetComponent<Agent>();
                    }
                    
                    // En caso de que no se esté clickando nada, el método termina
                    else
                    {
                        return;
                    }

                    foreach (var npc in listNPCs)
                    {
                        SendNewTarget(npc, agent);
                    }
                    
                }
            }
        }
    }

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

            if (npc.GetComponent<PathFollowing>())
                npc.GetComponent<PathFollowing>().NewTarget(agent);

         //   if (npc.GetComponent<WallAvoidance>())
           //     npc.GetComponent<WallAvoidance>().NewTarget(agent);
            
            // Steering Combinados
            if (npc.GetComponent<Blended>())
                npc.GetComponent<Blended>().NewTarget(agent);
            
        } 
    }
    

    public virtual GameObject CreateInvisibleAgent(Vector3 positionSpawn)
    {
        string nombreAI = "AgenteInvisible";
        GameObject ai;
        if (!GameObject.Find(nombreAI))
        {
            ai = Instantiate(personajeInvisible, positionSpawn, Quaternion.identity);
            ai.AddComponent<AgentInvisible>();
            ai.GetComponent<AgentInvisible>().DrawGizmos = true;
            ai.GetComponent<MeshRenderer>().enabled = false;
            ai.name = nombreAI;
            return ai;
        }

        ai = GameObject.Find(nombreAI);
        ai.transform.position = positionSpawn;
        return ai;
    }
}