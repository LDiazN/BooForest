using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SteeringBehavior))]
public class BasicMovement : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private SteeringBehavior steeringBehavior;
    private bool _queued = false;

    private Vector2 _heading = Vector2.right;
    private Vector2 _side = Vector2.up;

    private Vector2 _velocity = Vector2.zero;
    public  Vector2 velocity { get { return _velocity; } }

    [SerializeField()]
    private float _maxSpeed = 10.0f;
    public float maxSpeed { get { return _maxSpeed; } }

    [SerializeField()]
    private float _mass = 10.0f;

    [SerializeField()]
    private float _maxForce = 100.0f;
    private float maxForce { get { return _maxForce;} }

    private void Awake()
    {
        // setup behavior component
        steeringBehavior = GetComponent<SteeringBehavior>();
        if (steeringBehavior == null)
            Debug.LogError("MISSING STEERING BEHAVIOR");

        _rb2d = GetComponent<Rigidbody2D>();    
    }

    public void Move(Vector2 vel)
    {
        _queued = true;
        _velocity = vel;
    }

    public void Update()
    {
        // Compute acceleration
        var force = Vector2.ClampMagnitude( steeringBehavior.Calculate(), _maxForce);
        var acceleration = force * (1.0f / _mass);

        // Compute velocity and clamp it if necessary
        _velocity += acceleration * Time.deltaTime;
        if (_velocity.sqrMagnitude > _maxSpeed * _maxSpeed)
            _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed);
        Debug.Log("My velocity is " + _velocity);
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

    private void FixedUpdate()
    {
        if (_queued)
        {
            _rb2d.MovePosition((Vector2)transform.position + _velocity * Time.fixedDeltaTime);
            _queued = false;
            _velocity = Vector2.zero;
        }
    }
}
