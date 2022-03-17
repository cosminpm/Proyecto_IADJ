using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormacionReloj : FormationManager
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
        updateSlots();
    }

    void Update()
    {
        updateSlots();
    }


    protected override void updateSlots()
    {

        // Obtenemos el lider de la formación
        Agent lider = getLeader();

        lider.Orientation *= -1;
        for (int i = 1; i < listaSlotsOcupados.Count; i++)
        {

            //Agent aux = listaSlotsOcupados[i].GetCharacter();

            LocalizacionSlot aux = getSlotLocation(i);

            Vector3 position = new Vector3(Mathf.Cos(lider.Orientation) * aux.GetPosicion().x - Mathf.Sin(lider.Orientation) * aux.GetPosicion().z, 0,
                                              Mathf.Sin(lider.Orientation) * aux.GetPosicion().x + Mathf.Cos(lider.Orientation) * aux.GetPosicion().z);


            agentesInvisibles[i].Orientation = -(lider.Orientation + aux.GetOrientacion());

            // Seek to relative position.
            if (lider.Velocity.magnitude >= 1)
            {
                agentesInvisibles[i].Position = position + lider.Position;

                listaSlotsOcupados[i].GetCharacter().GetComponent<Arrive>().NewTarget(agentesInvisibles[i]);
            }

            // Align
            // listaSlotsOcupados[i].GetCharacter().GetComponent<Align>().NewTarget(agente);
        }

    }

    // Calcula el numero de slots ocupados en la formación.
    protected int calculateNumberOfSlots(List<SlotAssignment> listaSlots)
    {
        return listaSlots.Count;
    }

    public LocalizacionSlot getSlotLocation(int slotNumber)
    {

        LocalizacionSlot ls = new LocalizacionSlot();

        for (int i = 0; i < maxSlots; i++)
        { 
        
        
        
        }

        if (slotNumber == 0)
        {
            ls.SetPosicion(Vector3.zero);
            ls.SetOrientacion(0);
        }
        else if (slotNumber == 1)
        {
            ls.SetPosicion(new Vector3(distanciaSeparacion, 0, 0));
            ls.SetOrientacion(0);
        }
        else if (slotNumber == 2)
        {
            ls.SetPosicion(new Vector3(distanciaSeparacion, 0, -distanciaSeparacion));
            ls.SetOrientacion(0);
        }
        else
        {
            ls.SetPosicion(new Vector3(0, 0, -distanciaSeparacion));
            ls.SetOrientacion(0);
        }

        return ls;
    }

    // Devuelve la posición de un slot concreto dentro de la formación. TODO NOMBRE?
    protected override Agent getAgentSlotLocation(int slotNumber)
    {

        if (listaSlotsOcupados.Count >= slotNumber)
        {

            Agent agente = getCharacterBySlotNumber(slotNumber);


            return agente;
        }

        return null;
    }

    // Devuelve true si quedan slots libres en la formación
    protected override bool soportaSlots(int slotNumber)
    {

        return maxSlots >= slotNumber;
    }
}

