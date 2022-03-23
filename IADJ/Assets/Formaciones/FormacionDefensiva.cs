using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormacionDefensiva : FormationManager
{
    [SerializeField] private int distanciaSeparacion;
    [SerializeField] private float radioExteriorInvisible;
    [SerializeField] private float radioInteriorInvisible;
    [SerializeField] private Agent liderAux;

    private int maxSlots = 11;
    private bool inWander = false;
    private List<AgentInvisible> agentesInvisibles;


    public struct LocalizacionSlot
    {
        private Vector3 posicion;
        private float orientacion;

        // Creamos los getters y los setters.
        public void SetPosicion(Vector3 p) { posicion = p; }
        public Vector3 GetPosicion() { return posicion; }

        public void SetOrientacion(float o) { orientacion = o; }
        public float GetOrientacion() { return orientacion; }

    }
    // Start is called before the first frame update
    void Start()
    {
        agentesInvisibles = new List<AgentInvisible>();

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject go = new GameObject("agenteInvisibleNumero : " + i);
            AgentInvisible invisible = go.AddComponent<AgentInvisible>() as AgentInvisible;
            invisible.GetComponent<AgentInvisible>().DrawGizmos = true;
            invisible.ArrivalRadius = radioExteriorInvisible;
            invisible.InteriorRadius = radioInteriorInvisible;

            agentesInvisibles.Add(invisible);
        }

        inicializarSlots();
    }

    void Update()
    {
        inicializarSlots();
        if (listaAgents.Count > 0)
            updateSlots();
    }

    protected override void updateSlots()
    {
        // Obtenemos el l�der de la formaci�n.
        Agent lider = getLeader();
        GameObject objetoLider = GameObject.Find(lider.name);

        // Descontamos un frame hasta que el l�der pase a 
        // tener el steering wander.
        if (lider.Velocity.magnitude < 1)
            timeWander--;

        if (timeWander <= 0 && inWander == false)
        {
            Destroy(lider.GetComponent<Arrive>());

            Destroy(lider.GetComponent<Align>());

            objetoLider.AddComponent<Wander>();
            //lider.GetComponent<AgentNPC>().listSteerings.Add((Face)Wander);

            inWander = true;
        }
        /*
        else
        {
            if (objetoLider.GetComponent<Wander>() != null)
                Destroy(objetoLider.GetComponent<Wander>());

            if (objetoLider.GetComponent<Arrive>() == null)
            {
                objetoLider.AddComponent<Arrive>();
                //objetoLider.GetComponent<Arrive>().NewTarget();

            }


            if (objetoLider.GetComponent<Align>() == null)
                objetoLider.AddComponent<Align>();
        }*/
   
        if (listaAgents.Count == maxSlots)
        {
            for (int i = 1; i < listaSlotsOcupados.Count; i++)
            {
                LocalizacionSlot aux = getSlotLocation(i);

                float orientacion = lider.Orientation * Mathf.PI / 180;

                Vector3 position = new Vector3((-Mathf.Sin(orientacion) * aux.GetPosicion().z - Mathf.Cos(orientacion) * aux.GetPosicion().x), 0,
                                               (Mathf.Sin(orientacion) * aux.GetPosicion().x - Mathf.Cos(orientacion) * aux.GetPosicion().z));

                agentesInvisibles[i].Orientation = lider.Orientation + aux.GetOrientacion() - driftoffset.GetOrientation();

                // Seek to relative position.
                if (lider.Velocity.magnitude >= 1)
                {
                    agentesInvisibles[i].Position = position + lider.Position - driftoffset.GetPosition();
                    listaSlotsOcupados[i].GetCharacter().GetComponent<Arrive>().NewTarget(agentesInvisibles[i]);
                }

                // Align
                //listaSlotsOcupados[i].GetCharacter().GetComponent<Align>().NewTarget(agentesInvisibles[i]);
            }
        }

        else
        {
            for (int i = 1; i < listaSlotsOcupados.Count; i++)
            {
                listaSlotsOcupados[i].GetCharacter().GetComponent<Arrive>().NewTarget(lider.GetComponent<Arrive>().getTarget());
            }
        }
    }

    // Calcula el numero de slots ocupados en la formaci�n.
    protected int calculateNumberOfSlots(List<SlotAssignment> listaSlots)
    {
        return listaSlots.Count;
    }

    public LocalizacionSlot getSlotLocation(int slotNumber)
    {

        LocalizacionSlot ls = new LocalizacionSlot();

        if (slotNumber <= 5)
            ls.SetPosicion(new Vector3(distanciaSeparacion * (Mathf.Cos(slotNumber) + slotNumber * Mathf.Sin(slotNumber)), 0, -distanciaSeparacion * (Mathf.Sin(slotNumber) - slotNumber * Mathf.Cos(slotNumber))));
        else
        {
            int slotNumberAux = ((slotNumber % 5) + 1);
            ls.SetPosicion(new Vector3(-distanciaSeparacion * (Mathf.Cos(slotNumberAux) + slotNumberAux * Mathf.Sin(slotNumberAux)), 0, -distanciaSeparacion * (Mathf.Sin(slotNumberAux) - slotNumberAux * Mathf.Cos(slotNumberAux))));
        }

        ls.SetOrientacion(0);

        return ls;
    }

    // Devuelve la posici�n de un slot concreto dentro de la formaci�n. 
    protected override Agent getAgentSlotLocation(int slotNumber)
    {

        if (listaSlotsOcupados.Count >= slotNumber)
        {
            Agent agente = getCharacterBySlotNumber(slotNumber);
            return agente;
        }

        return null;
    }

    // Devuelve true si quedan slots libres en la formaci�n
    protected override bool soportaSlots(int slotNumber)
    {
        return maxSlots >= slotNumber;
    }

    // Devolvemos el DriftOffset de la formaci�n.
    protected override DriftOffset getDriftOffset(List<SlotAssignment> s)
    {
        // Declaramos el centro de masa.
        DriftOffset center = new DriftOffset();

        // Recorremos cada asignaci�n y a�adimos su contribuci�n al centro.
        foreach (SlotAssignment sa in s)
        {
            LocalizacionSlot ls = getSlotLocation(sa.GetSlotNumber());
            center.SetPosition(center.GetPosition() + ls.GetPosicion());
            center.SetOrientation(center.GetOrientation() + ls.GetOrientacion());
        }

        // Dividimos entre el n�mero de asiganciones.
        center.SetPosition(center.GetPosition() / s.Count);
        center.SetOrientation(center.GetOrientation() / s.Count);

        return center;
    }
}
