using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// En principio cada tipo de personaje podrá definir su propio GUI...

public class GUIManager : MonoBehaviour
{


    private NPC npc;

    public HealthBar healthBar;

    public UIBar actionBar;

     void Update(){
        healthBar.UpdateBar(npc.Unit.CurrentHealthPoints);
    }

    public void Initialize(){
        npc = GetComponent<NPC>();
        healthBar.SetMaxValue(npc.Unit.HealthPointsMax);
        actionBar.SetMaxValue(360);

    }


    public void UpdateBarAction(int cooldwnTime)
    {
        actionBar.UpdateBar(cooldwnTime);
    }



    public void UpdateStateImagen(State oldState, State newState){
        if ( oldState != null)
            oldState.stateImage.enabled = false;
            
        newState.stateImage.enabled = true;
    }
    






  


    






 
}

   