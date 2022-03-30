using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormacionOfensiva : FormationManager
{
    [SerializeField] private int distanciaSeparacion;
    [SerializeField] private float radioExteriorInvisible;
    [SerializeField] private float radioInteriorInvisible;

    private int maxSlots = 11;

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
        // Creamos la lista de slots.
        agentesInvisibles = new List<AgentInvisible>();

        // Para cada slot disponible creamos un agente invisible que lo 
        // represente. 
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject go = new GameObject("agenteInvisibleNumero " + i);
            AgentInvisible invisible = go.AddComponent<AgentInvisible>() as AgentInvisible;
            invisible.GetComponent<AgentInvisible>().DrawGizmos = true;
            invisible.ArrivalRadius = radioExteriorInvisible;
            invisible.InteriorRadius = radioInteriorInvisible;

            agentesInvisibles.Add(invisible);
        }

        // Asignamos los personajes disponibles (si los hay)
        // a los slots que acabamos de crear.
        inicializarSlots();
    }

    void Update()
    {

        // Volvemos a asignar los personajes a los slots en cada frame.
        // Esto lo hacemos por si se seleecionan nuevos personajes
        // en tiempo de ejecución.
        inicializarSlots();

        // Si tenemos agentes seleccionados, actualizamos la posición de los 
        // slots.
        if (listaAgents.Count > 0)
            updatesSlotsLider();
    }

    // Esta función nos servirá para actualizar la posición de los slots.
    protected override void updatesSlotsLider()
    {

        // Obtenemos el lider de la formaci�n
        Agent lider = getLeader();
        GameObject objetoLider = GameObject.Find(lider.name);

        // Si el líder está quieto, descontamos frames para que
        // pase a estado Wander.
        if (lider.Velocity.magnitude < 1)
            timeWander--;

        // Si el líder está en estado Wander, restamos un 
        // frame para que se detenga.
        if (lider.GetComponent<Wander>().enabled == true && timeWander <= 0)
            timeToStop--;

        // Si hemos llegado a 0 y hay un target activo, eliminamos el 
        // target y entramos en estado Wander.
        if (timeWander <= 0 && GameObject.Find("AgenteInvisible"))
        {
            lider.GetComponent<Arrive>().enabled = false;
            lider.GetComponent<Face>().enabled = false;

            lider.GetComponent<Wander>().enabled = true;
        }

        // Si ha llegado el momento de parar, asignamos la velocidad
        // del líder a 0.
        if (timeToStop <= 0)
        {
            lider.GetComponent<Wander>().enabled = false;
            lider.Velocity = Vector3.zero;

            // Si estamos parados y encontramos un nuevo
            // target, salimos del estado Wander y reactivamos el Face
            // y el Arrive y ponemos el contador de frames a su estado original.
            if (GameObject.Find("AgenteInvisible"))
            {
                lider.GetComponent<Arrive>().enabled = true;
                lider.GetComponent<Face>().enabled = true;

                timeWander = 3600;
            }

            else
                lider.GetComponent<Wander>().enabled = true;

            timeToStop = 3600;
        }


        // Si todos los slots están ocupados...
        if (listaSlotsOcupados.Count == maxSlots)
        {
            // Para cada slot, le asignamos su posición y orientación 
            // correspondientes en la formación.
            for (int i = 1; i < listaSlotsOcupados.Count; i++)
            {
                LocalizacionSlot aux = getSlotLocation(i);

                float orientacion = -lider.Orientation * Mathf.PI / 180;

                Vector3 position = new Vector3(-(Mathf.Cos(orientacion) * aux.GetPosicion().z - Mathf.Sin(orientacion) * aux.GetPosicion().x), 0,
                                               -(Mathf.Cos(orientacion) * aux.GetPosicion().x + Mathf.Sin(orientacion) * aux.GetPosicion().z));


                agentesInvisibles[i].Orientation = lider.Orientation + aux.GetOrientacion() - driftoffset.GetOrientation();

                // Arrive to relative position.
                agentesInvisibles[i].Position = position + lider.Position - driftoffset.GetPosition();
                listaSlotsOcupados[i].GetCharacter().GetComponent<Arrive>().NewTarget(agentesInvisibles[i]);

                // Si el NPC no está en movimiento, se alinea con la orientación de su slot.
                if (listaSlotsOcupados[i].GetCharacter().Velocity.magnitude == 0)
                {
                    listaSlotsOcupados[i].GetCharacter().GetComponent<Align>().NewTarget(agentesInvisibles[i]);
                }

                // Si lo está, se alineará con la orientación del líder.
                else
                {
                    listaSlotsOcupados[i].GetCharacter().GetComponent<Align>().NewTarget(lider);
                }
            }
        }

        // Si todos los slots no están ocupados, hacemos que cada personaje de la formación
        // aplique un Arrive al target del líder.
        else
        {
            for (int i = 1; i < listaSlotsOcupados.Count; i++)
            {
                listaSlotsOcupados[i].GetCharacter().GetComponent<Arrive>().NewTarget(lider.GetComponent<Arrive>().getTarget());
            }
        }
    }


    // Función para calcular la posición del slot de acuerdo a la formación.
    public LocalizacionSlot getSlotLocation(int slotNumber)
    {
        // Creamos una instancia de LocalizacionSlot (tupla posición-orientación).
        LocalizacionSlot ls = new LocalizacionSlot();

        // Dividimos la formación en dos grupos para hacerla de forma simétrica alrededor del
        // líder.
        if (slotNumber <= 5)
        {
            ls.SetPosicion(new Vector3(distanciaSeparacion * (Mathf.Cos(slotNumber) + slotNumber * Mathf.Sin(slotNumber)), 0, -distanciaSeparacion * (Mathf.Sin(slotNumber) - slotNumber * Mathf.Cos(slotNumber))));
            ls.SetOrientacion(90);
        }

        else
        {
            int slotNumberAux = ((slotNumber % 5) + 1);
            ls.SetPosicion(new Vector3(distanciaSeparacion * (Mathf.Cos(slotNumberAux) + slotNumberAux * Mathf.Sin(slotNumberAux)), 0, distanciaSeparacion * (Mathf.Sin(slotNumberAux) - slotNumberAux * Mathf.Cos(slotNumberAux))));
            ls.SetOrientacion(-90);
        }

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
