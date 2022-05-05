
using UnityEngine;
using UnityEngine.UI;

// En principio cada tipo de personaje podrá definir su propio GUI...

public class HealthBar : UIBar
{
    public Slider temporalDamage;
    private float _oldValue;
    private float _hitDelay = 0.6f;
    private bool _update = false;

    private float _nextUpdate;

    public override void SetMaxValue(float value){
        base.SetMaxValue(value);
        temporalDamage.maxValue = value;
    }

    public override void UpdateBar(float value){
        base.UpdateBar(value);

        if ( value < _oldValue && !_update){
            _nextUpdate = Time.time + _hitDelay;
            _update = true;
        }

        if ( Time.time > _nextUpdate)
        {
            if ( value < _oldValue)
                _oldValue -= 1;
            else 
                _oldValue = value;
            if ( _oldValue == value)
                _update = false;
        }

        temporalDamage.value = _oldValue;

    }

}