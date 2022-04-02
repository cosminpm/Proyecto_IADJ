using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AgentNPC))]

public abstract class SteeringBehaviour : MonoBehaviour
{

    public enum Group{
        Colision = 0,
        Separation = 1,
        Persecution = 2    
    }

    protected string nameSteering = "no steering";

    // El peso asociado a cada steering.
    public float weight;

    // El grupo al que pertenece el steering
    public Group group;

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
