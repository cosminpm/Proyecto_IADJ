using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbitroPrioritarioDinamico: ArbitroManager {

    [Header("Prioridad grupo colisión")]
    public int prioridadGrupoColision = 0;

    [Header("Prioridad grupo separación")]
    public int prioridadGrupoSeparacion = 1;

    [Header("Prioridad grupo persecucción")]
    public int prioridadGrupoPersecuccion = 2;

    private SortedList<int, ArbitroPonderado> groups;

    // Prioridad del 
    public float epsilon = 1f;


    public void Start(){
        Inicializar();
        groups = new SortedList<int, ArbitroPonderado>();
        CreateGroups();
    }

    private void CreateGroups(){

        List<SteeringBehaviour> colision = new List<SteeringBehaviour>();
        List<SteeringBehaviour> separacion = new List<SteeringBehaviour>();
        List<SteeringBehaviour> persecuccion = new List<SteeringBehaviour>();

        // Recorro la lista de steerings del agente, añadiendolos a los grupos que pertenece
        foreach(var steer in listaBehaviours){

            switch (steer.Group)
            {
            case 0:
                colision.Add(steer);
                break;
            case 1:
                separacion.Add(steer);
                break;
            case 2:
                persecuccion.Add(steer);
                break;
            default:
                break;
            }
        }

        if ( colision.Count > 0)
            AddGroup(colision, prioridadGrupoColision);

        if ( separacion.Count > 0)
            AddGroup(separacion, prioridadGrupoSeparacion);

        if ( persecuccion.Count > 0) 
            AddGroup(persecuccion, prioridadGrupoPersecuccion);


    }

    private void AddGroup(List<SteeringBehaviour> list, int priority) {  
        ArbitroPonderado blended = new ArbitroPonderado();
        blended.UpdateList(list);
        groups.Add(priority,blended);

    }

    private void UpdateGroups(){

    }


    public override Steering GetSteering(Agent agent)
    {
        Steering steer = new Steering();

        foreach( var group in groups){
            
            steer = group.Value.GetSteering(agent);
            if ( Mathf.Abs(steer.linear.magnitude) > epsilon || Mathf.Abs(steer.angular) > epsilon)
            {
                return steer;
            }
        }
        return steer;
    }

}