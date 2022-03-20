using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationManager : MonoBehaviour
{
    // Estructura para indicar el personaje y el numero de slot que tiene 
    // asociado cada slot.

    public struct SlotAssignment
    {
        private Agent character;
        private int slotNumber;

        // Creamos los getters y los setters.
        public void SetCharacter(Agent agent) { character = agent; }
        public Agent GetCharacter() { return character; }

        public void SetSlotNumber(int slot) { slotNumber = slot; }
        public int GetSlotNumber() { return slotNumber; }

    }

    protected List<SlotAssignment> listaSlotsOcupados = new List<SlotAssignment>();

    [SerializeField] public List<Agent> listaAgents = new List<Agent>();

    protected void inicializarSlots()
    {
        // añadimos un caracter
        foreach ( var agente in listaAgents){
           addCharacter(agente);
        }
    }

    void Start(){
        inicializarSlots();
        updateSlots();
    }


    // Asigna un personaje a un slots. Si el 
    protected bool addCharacter(Agent agent){

        // Obtenemos el número de slots ocupados
        int slotsOcupados = listaSlotsOcupados.Count;
        // Si la formación soporta un npc más, se crea un slot, se le asigna 
        // un número de slot y el agente. Se añade este slot a la lista de slots
        // ocupados. Y por último se devuelve True.
        if ( soportaSlots(slotsOcupados+1) ){

            // TODO
            SlotAssignment slot = new SlotAssignment();
            slot.SetSlotNumber(slotsOcupados);
            slot.SetCharacter(agent);
            listaSlotsOcupados.Add(slot);
            return true;
        }
        // En caso contrario devolvemos false.
        return false;
    }

    // Actualizamos las asignaciones de cada personaje en la lista
    // de slots. Esto lo hacemos para eliminar slots intermedios vacios en las
    // formaciones.
    protected void updateSlotAssignments(){

        for( int i = 0; i < listaSlotsOcupados.Count; i++)
        {
            listaSlotsOcupados[i].SetSlotNumber(i);

        }

    }

    // Recorremos la lista de slots ocupados en busca del agente
    // que se pasa como parametro. Si se encuentra se elimina
    // la asignación que tenga dicho agente en la lista de slots.

    protected void removeCharacter(Agent agente){
        bool encontrado = false;
        int index = 0;    
        foreach ( var s in listaSlotsOcupados){

            if (s.GetCharacter() == agente){
                encontrado = true;
                break;
            }
            index++;      
        }
        if (encontrado){
            listaSlotsOcupados.RemoveAt(index);
            // Actualizamos la lista de slots.
            updateSlotAssignments();
        }
    }


    // Devuele el agenteNPC en la formación dado su slots.
    protected Agent getCharacterBySlotNumber(int numberSlot){

        return listaSlotsOcupados[numberSlot].GetCharacter(); 

    }

    protected Agent getLeader(){
        return listaSlotsOcupados[0].GetCharacter();
    }

    protected abstract Agent getAgentSlotLocation(int numberSlot);
    protected abstract bool soportaSlots(int numberSlot);
    protected abstract void updateSlots();


}
