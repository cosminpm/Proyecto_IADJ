using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentNPC))]
public abstract class SteeringBehaviour : MonoBehaviour
{

    protected string nameSteering = "no steering";

    public string NameSteering
    {
        set { nameSteering = value; }
        get { return nameSteering; }
    }

    public abstract Steering GetSteering(Agent agent);
    
    protected virtual void OnGUI()
    {
        //Para la depuración te puede interesar que se muestre el nombre
        // del steeringbehaviour sobre el personaje.
        // Te puede ser util Rect() y GUI.TextField()
        // https://docs.unity3d.com/ScriptReference/GUI.TextField.html
    }
}
