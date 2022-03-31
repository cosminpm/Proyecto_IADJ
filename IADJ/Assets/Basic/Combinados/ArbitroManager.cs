using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public abstract class ArbitroManager : MonoBehaviour {

    // Lista con los steerings del arbitro 
    protected List<SteeringBehaviour> listaBehaviours;


    public void Inicializar(){
        listaBehaviours = GetComponents<SteeringBehaviour>().ToList();
    }

    public void UpdateList(List<SteeringBehaviour> list){
        listaBehaviours = new List<SteeringBehaviour>(list);
    }
    
    public abstract Steering GetSteering(Agent agent);
}