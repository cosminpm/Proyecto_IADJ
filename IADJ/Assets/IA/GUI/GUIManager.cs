using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

// En principio cada tipo de personaje podrá definir su propio GUI...

public class GUIManager : MonoBehaviour
{

    private NPC npc;

    public HealthBar healthBar;

    public void Initialize(){
        npc = GetComponent<NPC>();
        healthBar.SetMaxValue(npc.Unit.HealthPointsMax);
    }


    void Update(){
        healthBar.UpdateBar(npc.Unit.CurrentHealthPoints);
    }

  


    






 
}

   