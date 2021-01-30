using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SteeringBehavior))]
public class SteeringMovement : BasicMovement
{
    private SteeringBehavior steeringBehavior;

    private Vector2 _heading = Vector2.right;
    public Vector2 heading { get { return _heading; } }
    private Vector2 _side = Vector2.up;
    public Vector2 side { get { return _side; } }


    [SerializeField()]
    private float _maxSpeed = 10.0f;
    public float maxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }

    [SerializeField()]
    private float _mass = 10.0f;

    [SerializeField()]
    private float _maxForce = 100.0f;
    private float maxForce { get { return _maxForce; } }

    protected override void Awake()
    {
        base.Awake();

        // setup behavior component
        steeringBehavior = GetComponent<SteeringBehavior>();
        if (steeringBehavior == null)
            Debug.LogError("MISSING STEERING BEHAVIOR");

    }

    // Update is called once per frame
    void Update()
    {
        // Compute acceleration
        var force = Vector2.ClampMagnitude(steeringBehavior.Calculate(), _maxForce);
        var acceleration = force * (1.0f / _mass);

        // Compute velocity and clamp it if necessary
        _velocity += acceleration * Time.deltaTime;
        if (_velocity.sqrMagnitude > _maxSpeed * _maxSpeed)
            _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed);

        // Update position:
        Move(_velocity);

        // Update heading and side if speed is high enough
        if (_velocity.sqrMagnitude > 0.0001f)
        {
            _heading = _velocity.normalized;
            _side = Vector3.Cross(Vector3.forward, _heading);
        }

        Debug.DrawLine(transform.position, transform.position + ((Vector3)_heading), Color.red);
        Debug.DrawLine(transform.position, transform.position + ((Vector3)_side), Color.blue);
    }

    public void Stop() => _velocity = Vector2.zero;
}
