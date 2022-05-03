
using UnityEngine;
using UnityEngine.UI;

// En principio cada tipo de personaje podrá definir su propio GUI...

public class UIBar : MonoBehaviour
{

    public Slider bar;

    
    public virtual void SetMaxValue(float value){
        bar.maxValue = value;
    }

    public virtual void UpdateBar(float value){
        bar.value = value;
    }



 
}

   