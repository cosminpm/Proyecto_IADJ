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
        public void SetCharacter(Agent agent)
        {
            character = agent;
        }

        public Agent GetCharacter()
        {
            return character;
        }

        public void SetSlotNumber(int slot)
        {
            slotNumber = slot;
        }

        public int GetSlotNumber()
        {
            return slotNumber;
        }
    }

    // Estructura en la que almacenaremos la posición y la orientación 
    // necesarias para evitar derrapes.
    public struct DriftOffset
    {
        Vector3 position;
        float orientation;

        // Creamos los getters y los setters.
        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetOrientation(float ori)
        {
            orientation = ori;
        }

        public float GetOrientation()
        {
            return orientation;
        }
    }


    protected List<SlotAssignment> listaSlotsOcupados = new List<SlotAssignment>();
    protected DriftOffset driftoffset;
    protected int timeWander = 3600;

    // Variable que usaremos para, cuando el líder esté
    // en Wander, controlar el tiempo restante hasta que tenga 
    // que parar.
    protected int timeToStop = 3600;
    
    public List<Agent> listaAgents = new List<Agent>();
    // Variable para controlar el tiempo que falta (en frames)
    // hasta que el líder cambie su steering a wander.

    public int ModoFormacion = 0;

    void Start()
    {
        inicializarSlots();
        
        
        if (ModoFormacion == 0)
            UpdatesSlotsLider();
        else if (ModoFormacion == 1)
        {
            UpdateSlotsLRTA();
        }
    }

    // Una vez inicializada la lista de agentes asignamos, si se puede, un slot a cada agente.
    protected void inicializarSlots()
    {
        // Obtenemos la lista de NPCs seleccionados.
        SeleccionarObjetivos so = GameObject.Find("Controlador").GetComponent<SeleccionarObjetivos>();

        // Para cada personaje en la lista...
        foreach (var ag in so.getListNPCs())
        {
            // Si el personaje seleccionado no estaba en la lista
            // de NPCs de la formación, lo añadimos.
            if (!listaAgents.Contains(ag.GetComponent<Agent>()))
            {
                listaAgents.Add(ag.GetComponent<Agent>());

                // Además, le asignamos un slot de la formación
                // a dicho personaje.
                addCharacter(ag.GetComponent<Agent>());
            }
        }

        // Si la lista de seleccionados tiene menos elementos
        // que la lista de personajes que componen la formación,
        // entonces comprobamos cuál es el personaje de diferencia
        // y lo eliminamos de la lista de NPCs de la formación.
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
    protected bool addCharacter(Agent agent)
    {
        // Obtenemos el número de slots ocupados.
        int slotsOcupados = listaSlotsOcupados.Count;

        // Si la formación soporta un NPC más, se crea un slot, se le asigna 
        // un número de slot y el agente. Se añade este slot a la lista de slots
        // ocupados. Y por último se devuelve True.
        if (soportaSlots(slotsOcupados + 1))
        {
            SlotAssignment slot = new SlotAssignment();
            slot.SetSlotNumber(slotsOcupados);
            slot.SetCharacter(agent);
            listaSlotsOcupados.Add(slot);

            return true;
        }

        // En caso contrario devolvemos False.
        return false;
    }

    // Actualizamos las asignaciones de cada personaje en la lista
    // de slots. Esto lo hacemos para eliminar slots intermedios vacios en las
    // formaciones.
    protected void updateSlotAssignments()
    {
        for (int i = 1; i < listaSlotsOcupados.Count; i++)
        {
            listaSlotsOcupados[i].SetSlotNumber(i);
        }

        driftoffset = getDriftOffset(listaSlotsOcupados);
    }

    // Recorremos la lista de slots ocupados en busca del agente
    // que se pasa como parámetro. Si se encuentra, se elimina
    // la asignación que tenga dicho agente en la lista de slots.
    protected void removeCharacter(Agent agente)
    {
        bool encontrado = false;
        int index = 0;
        foreach (var s in listaSlotsOcupados)
        {
            if (s.GetCharacter() == agente)
            {
                encontrado = true;
                break;
            }

            index++;
        }

        if (encontrado)
        {
            listaSlotsOcupados.RemoveAt(index);
            listaAgents.RemoveAt(index);
            // Actualizamos la lista de slots.
            updateSlotAssignments();
        }
    }


    // Devuele el agenteNPC en la formación dado su slots.
    protected Agent getCharacterBySlotNumber(int numberSlot)
    {
        return listaSlotsOcupados[numberSlot].GetCharacter();
    }

    // función para obtener el personaje que esté asignado al primer 
    // slot de la formación. Es decir, el líder.
    protected Agent getLeader()
    {
        return listaSlotsOcupados[0].GetCharacter();
    }


    protected abstract void UpdateSlotsLRTA();
    protected abstract Agent getAgentSlotLocation(int numberSlot);
    protected abstract bool soportaSlots(int numberSlot);
    protected abstract void UpdatesSlotsLider();
    protected abstract DriftOffset getDriftOffset(List<SlotAssignment> s);
}