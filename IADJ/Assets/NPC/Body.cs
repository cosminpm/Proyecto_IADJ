using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] protected float _mass = 1;
    [SerializeField] protected float _maxSpeed = 1;
    [SerializeField] protected float _maxRotation = 1;
    [SerializeField] protected float _maxAcceleration = 1;
    [SerializeField] protected float _maxAngularAcc = 1;
    [SerializeField] protected float _maxForce = 1;

    protected Vector3 _acceleration; // aceleración lineal
    private Vector3 _velocity; // velocidad lineal
    protected float _rotation; // velocidad angular
    protected float _speed; // velocidad escalar

    // Vamos a guardar la orientacion como angulos de 0 a 360
    [SerializeField] protected float _orientation; // 'posición' angular

    public float Mass
    {
        get { return _mass; }
        set { _mass = value; }
    }

    public float MaxForce
    {
        get { return _maxForce; }
        set { _maxForce = Mathf.Max(0, value); }
    }

    public float MaxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = Mathf.Max(0, value); }
    }

    public Vector3 Velocity
    {
        get
        {
            _velocity = LimitVelocity(_velocity);
            return _velocity;
        }
        set
        {
            _velocity = LimitVelocity(value);
        }
    }

    public float MaxRotation
    {
        get { return _maxRotation; }
        set { _maxRotation = value; }
    }

    public float Rotation
    {
        get { return _rotation; }
        set
        {
            if (Mathf.Abs(value) < 0.1)
            {
                _rotation = 0;
            }
            _rotation = value;
        }
    }

    public float MaxAcceleration
    {
        get { return _maxAcceleration; }
        set { _maxAcceleration = Mathf.Max(0, value); }
    }

    public Vector3 Acceleration
    {
        get { return _acceleration; }
        set { _acceleration = value; }
    }

    // public Vector3 Position. Recuerda. Esta es la única propiedad que trabaja sobre transform.
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // Se pasa ese angulo
    public float Orientation
    {
        get { return _orientation; }
        set
        {
            _orientation = value;
            
        }
    }
    public float MaxAngularAcceleartion
    {
        get { return _maxAngularAcc; }
        set
        {
            _maxAcceleration = value;
        }
    }
    

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    // TE PUEDEN INTERESAR LOS SIGUIENTES MÉTODOS.
    // Quita o añade todos los que sean referentes a la parte física.
    // public float Heading()
    // public static float MapToRange(float rotation, Range r)
    // public float MapToRange(Range r)
    // public float PositionToAngle()
    // public Vector3 OrientationToVector()
    // public Vector3 VectorHeading()
    // public float GetMiniminAngleTo(Vector3 rotation)
    // public void ResetOrientation()
    // public float PredictNearestApproachTime(Bodi other, float timeInit, float timeEnd)
    // public float PredictNearestApproachDistance3(Bodi other, float timeInit, float timeEnd)

    private Vector3 LimitVelocity(Vector3 v)
    {
        
        _velocity = v;
        if (_velocity.magnitude > _maxSpeed)
        {
            _velocity = _velocity.normalized * _maxSpeed;
        }
           
        return _velocity;
    }
}