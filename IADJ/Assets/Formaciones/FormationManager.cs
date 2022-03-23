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

    // Estructura en la que almacenaremos la posición y la orientación 
    // necesarias para evitar derrapes.
    public struct DriftOffset
    {
        Vector3 position;
        float orientation;

        // Creamos los getters y los setters.
        public void SetPosition(Vector3 pos) { position = pos; }
        public Vector3 GetPosition() { return position; }

        public void SetOrientation(float ori) { orientation = ori; }
        public float GetOrientation() { return orientation; }
    }

    protected List<SlotAssignment> listaSlotsOcupados = new List<SlotAssignment>();
    public List<Agent> listaAgents = new List<Agent>();
    protected DriftOffset driftoffset;

    // Variable para controlar el tiempo que falta
    // hasta que el líder cambie su steering a wander.
    protected int timeWander = 120; //3600;

    void Start()
    {
        inicializarSlots();
        updateSlots();
    }

    // Una vez inicializada la lista de agentes asignamos, si se puede, un slot a cada agente.
    protected void inicializarSlots()
    {
        SeleccionarObjetivos so = GameObject.Find("Controlador").GetComponent<SeleccionarObjetivos>();

        foreach (var ag in so.getListNPCs())
        {
            if (!listaAgents.Contains(ag.GetComponent<Agent>()))
            {
                listaAgents.Add(ag.GetComponent<Agent>());
                addCharacter(ag.GetComponent<Agent>());
            }
        }

        if (so.getListNPCs().Count < listaAgents.Count)
        {
            List<Agent> listaAux = new List<Agent>();
            foreach (var ag in so.getListNPCs())
            {
                listaAux.Add(ag.GetComponent<Agent>());
            }

            Agent AEliminar = new AgentNPC();
            bool c = false;
            foreach (var a in listaAgents)
            {
                if (!listaAux.Contains(a))
                {
                    c = true;
                    AEliminar = a;
                    break;
                }
            }

            if (c)
                removeCharacter(AEliminar);   
        }
    }

    // Asigna un personaje a un slots. Si la formación soporta el número de 
    // agentes ya asignados más uno.
    protected bool addCharacter(Agent agent){

        // Obtenemos el número de slots ocupados
        int slotsOcupados = listaSlotsOcupados.Count;

        // Si la formación soporta un npc más, se crea un slot, se le asigna 
        // un número de slot y el agente. Se añade este slot a la lista de slots
        // ocupados. Y por último se devuelve True.
        if ( soportaSlots(slotsOcupados + 1) ){
            SlotAssignment slot = new SlotAssignment();
            slot.SetSlotNumber(slotsOcupados);
            slot.SetCharacter(agent);
            Debug.Log("INSERTANDO PERSONAJE: " + slot.GetCharacter().name);


            listaSlotsOcupados.Add(slot);
            Debug.Log("EL PERSONAJE \"" + slot.GetCharacter().name + "\" HA SIDO ASIGNADO");
            Debug.Log("LA LISTA AHORA TIENE TAMAÑO: " + listaSlotsOcupados.Count);

            return true;
        }

        // En caso contrario devolvemos false.
        return false;
    }

    // Actualizamos las asignaciones de cada personaje en la lista
    // de slots. Esto lo hacemos para eliminar slots intermedios vacios en las
    // formaciones.
    protected void updateSlotAssignments(){

        for( int i = 1; i < listaSlotsOcupados.Count; i++)
        {
            listaSlotsOcupados[i].SetSlotNumber(i);
        }

        driftoffset = getDriftOffset(listaSlotsOcupados);
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
            listaAgents.RemoveAt(index);
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
    protected abstract DriftOffset getDriftOffset(List<SlotAssignment> s);

}
