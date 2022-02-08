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
    protected float _angularAcc; // aceleración angular
    private Vector3 _velocity; // velocidad lineal
    protected float _rotation; // velocidad angular
    protected float _speed; // velocidad escalar

    // Vamos a guardar la orientacion como angulos de 0 a 360
    protected float _orientation; // 'posición' angular

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
            _velocity = limitVelocity(_velocity);
            return _velocity;
        }
        set { _velocity = limitVelocity(value); }
    }

    public float MaxRotation
    {
        get { return _maxRotation; }
        set { _maxRotation = value; }
    }

    public float Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
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

    public float AngularAcc
    {
        get { return _angularAcc; }
        set { _angularAcc = value; }
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
            // TODO: Creo que aquí es donde hay que cambiar la rotación según los ángulos
            if (_velocity.magnitude > 0)
                _orientation = Mathf.Atan2(Velocity.x, Velocity.z);
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

    private Vector3 limitVelocity(Vector3 v)
    {
        _velocity = v;
        if (_velocity.magnitude > _maxSpeed)
            _velocity = _velocity.normalized * _maxSpeed;
        return _velocity;
    }
}