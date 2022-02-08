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

    [SerializeField] private bool _drawGizmos;
    // AÑADIR LAS PROPIEDADES PARA ESTOS ATRIBUTOS. SI LO VES NECESARIO.

    [SerializeField] private float _anchuraCirculo = 3;
    [SerializeField] private float _anchuraLinea = 3;
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

    public void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            // Circulo interior
            Handles.color = Color.green;
            Handles.DrawWireDisc(Position, Vector3.up, _interiorRadius, _anchuraCirculo);
            // Circulo exterior
            Handles.color = new Color(0.0431f, 0.423f, 0, 1);
            Handles.DrawWireDisc(Position, Vector3.up, ArrivalRadius, _anchuraCirculo);

            // Velocidad
            Handles.color = Color.red;
            Handles.DrawLine(Position, Position + Velocity, _anchuraLinea);

            // Angulo interior
            Handles.color = Color.cyan;
            Quaternion intRot1 = Quaternion.Euler(0, _interiorAngle * Mathf.Rad2Deg, 0);
            Quaternion intRot2 = Quaternion.Euler(0, -_interiorAngle * Mathf.Rad2Deg, 0);
            var position = transform.position;
            var forward = transform.forward;
            Handles.DrawLine(position, position + intRot1 * forward);
            Handles.DrawLine(position, position + intRot2 * forward);

            // Angulo Exterior
        }
    }
}