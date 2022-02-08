using System;
using System.Collections;
using System.Collections.Generic;
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
        sendOrder();
    }
    private void SelectNPCs()
    {
        // Comprobamos si el ratón golpea a algo en el escenario.
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
    }

    private void sendOrder()
    {
        if (Input.GetMouseButtonUp(1))
        {
            // Comprobamos si el ratón golpea a algo en el escenario.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 targetTerrain = hitInfo.point;
                // Si se esta chocando algo...
                if (hitInfo.collider != null)
                {
                    foreach (var npc in listNPCs)
                    {
                        if (hitInfo.collider.CompareTag("Terrain"))
                        {
                            if (npc.GetComponent<Seek>())
                                npc.GetComponent<Seek>().NewTarget(targetTerrain);
                            if (npc.GetComponent<Flee>())
                                npc.GetComponent<Flee>().NewTarget(targetTerrain);
                        }
                        else if (hitInfo.collider.CompareTag("Protagonista") || hitInfo.collider.CompareTag("NPC"))
                        {
                            if (npc.GetComponent<VelocityMatching>())
                                npc.GetComponent<VelocityMatching>().NewTarget(hitInfo.transform.gameObject.GetComponent<AgentPlayer>());
                            if (npc.GetComponent<Arrive>())
                                npc.GetComponent<Arrive>().NewTarget(hitInfo.transform.gameObject.GetComponent<AgentPlayer>());
                            if (npc.GetComponent<Align>())
                                npc.GetComponent<Align>().NewTarget(hitInfo.transform.gameObject.GetComponent<AgentPlayer>());
                        }
                    }
                }
            }
        }
    }
}