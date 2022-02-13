using System;
using UnityEditor;
using UnityEngine;

[AddComponentMenu("Steering/InteractiveObject/Agent")]
public abstract class Agent : Body
{
    [Tooltip("Radio interior de la IA")] [SerializeField]
    protected float _interiorRadius = 1f;

    [Tooltip("Radio de llegada de la IA")] [SerializeField]
    protected float _arrivalRadius = 3f;

    [Tooltip("Ángulo interior de la IA")] [SerializeField]
    protected float _interiorAngle = 3.0f; // ángulo sexagesimal.

    [Tooltip("Ángulo exterior de la IA")] [SerializeField]
    protected float _exteriorAngle = 8.0f; // ángulo sexagesimal.
    
    [Tooltip("Dibujar todo")] [SerializeField]
    protected bool _drawGizmos;
    // AÑADIR LAS PROPIEDADES PARA ESTOS ATRIBUTOS. SI LO VES NECESARIO.

    [SerializeField] private float _anchuraCirculo = 3;
    [SerializeField] private float _anchuraLinea = 1.5f;
    public abstract void Awake();

    // AÑADIR LO NECESARIO PARA MOSTRAR LA DEPURACIÓN. Te puede interesar los siguientes enlaces.
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html
    // https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html
    // https://docs.unity3d.com/ScriptReference/Gizmos.DrawWireSphere.html
    // https://docs.unity3d.com/ScriptReference/Gizmos-color.html

    public float ExteriorAngle
    {
        get
        {
            if (_exteriorAngle < 0)
                _exteriorAngle = 0;
            if (_interiorAngle > _exteriorAngle)
                _exteriorAngle = _interiorAngle;
            return _exteriorAngle;
        }
    }

    public float InteriorAngle
    {
        get
        {
            if (_interiorAngle < 0)
                _interiorAngle = 0;
            if (_interiorAngle > _exteriorAngle)
                _exteriorAngle = _interiorAngle;
            return _interiorAngle;
        }
    }

    public float ArrivalRadius
    {
        get
        {
            if (_arrivalRadius < 0)
                _arrivalRadius = 0;
            if (_interiorRadius > _arrivalRadius)
                _arrivalRadius = _interiorRadius;
            return _arrivalRadius;
        }
    }


    public float InteriorRadius
    {
        get
        {
            if (_interiorRadius < 0)
                _interiorRadius = 0;
            if (_interiorRadius > _arrivalRadius)
                _arrivalRadius = _interiorRadius;
            return _interiorRadius;
        }
    }
    
    public bool DrawGizmos
    {
        get
        {
            return _drawGizmos;
        }

        set
        {
            _drawGizmos = value;
        }
    }
    
    public void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            // Radio interior
            Handles.color = Color.green;
            Handles.DrawWireDisc(Position, Vector3.up, _interiorRadius, _anchuraCirculo);
            // Radio exterior
            Handles.color = new Color(0.0431f, 0.423f, 0, 1);
            Handles.DrawWireDisc(Position, Vector3.up, ArrivalRadius, _anchuraCirculo);

            // Velocidad
            Handles.color = Color.red;
            Handles.DrawLine(Position, Position + Velocity, _anchuraLinea);

            // Angulo interior
            Handles.color = Color.cyan;
            Quaternion intRot1 = Quaternion.Euler(0, _interiorAngle, 0);
            Quaternion intRot2 = Quaternion.Euler(0, -_interiorAngle, 0);
            var position = transform.position;
            var forward = transform.forward;
            Handles.DrawLine(position, position + intRot1 * forward * ArrivalRadius, _anchuraLinea);
            Handles.DrawLine(position, position + intRot2 * forward * ArrivalRadius,_anchuraLinea);

            // Angulo exterior
            Handles.color = Color.black;
            Quaternion intRot12 = Quaternion.Euler(0, _exteriorAngle, 0);
            Quaternion intRot22 = Quaternion.Euler(0, -_exteriorAngle, 0);
            var position2 = transform.position;
            var forward2 = transform.forward;
            Handles.DrawLine(position2, position2 + intRot12 * forward2 * ArrivalRadius, _anchuraLinea);
            Handles.DrawLine(position2, position2 + intRot22 * forward2 * ArrivalRadius,_anchuraLinea);
        }
    }
}