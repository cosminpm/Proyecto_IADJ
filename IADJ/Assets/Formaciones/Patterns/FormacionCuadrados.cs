using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormacionCuadrados : FormationManager
{


    [SerializeField] private int distanciaSeparacion;


    private GameObject go;

    private int maxSlots = 8 ;



    // Start is called before the first frame update
    void Start()
    {
        inicializarSlots();
  
    }

    void Update(){

        updateSlots();
    }

    protected override void updateSlots(){

        // Obtenemos el lider de la formación
        Agent lider = getLeader();

        go = GameObject.Find("Controlador").GetComponent<SeleccionarObjetivos>().CreateInvisibleAgent(lider.Position);
        go.GetComponent<AgentInvisible>().DrawGizmos = true;

        Agent agente = go.GetComponent<AgentInvisible>();



        for (int i = 1; i < listaSlotsOcupados.Count ; i++){

            Agent aux = listaSlotsOcupados[i].GetCharacter();

            Vector3 position = new Vector3(   Mathf.Cos(lider.Orientation)*aux.Position.x-Mathf.Sin(lider.Orientation)*aux.Position.z ,0,
                                              Mathf.Sin(lider.Orientation)*aux.Position.x+Mathf.Cos(lider.Orientation)*aux.Position.z);
         
            agente.Position = position+lider.Position;
            agente.Orientation = lider.Orientation+aux.Orientation;

            // Seek to relative position.
           listaSlotsOcupados[i].GetCharacter().GetComponent<Seek>().NewTarget(agente);

            // Align
            // listaSlotsOcupados[i].GetCharacter().GetComponent<Align>().NewTarget(agente);
        }

    }

    // Calcula el numero de slots ocupados en la formación.
    protected int calculateNumberOfSlots(List<SlotAssignment> listaSlots){
        return listaSlots.Count;
    }


    // Devuelve la posición de un slot concreto dentro de la formación. TODO NOMBRE?
    protected override Agent getAgentSlotLocation(int slotNumber){

        if ( listaSlotsOcupados.Count >= slotNumber){

            Agent agente = getCharacterBySlotNumber(slotNumber);


            return agente;
        }

        return null;
    }

    // Devuelve true si quedan slots libres en la formación
    protected override bool soportaSlots(int slotNumber){

        return maxSlots >= slotNumber;
    }
}
