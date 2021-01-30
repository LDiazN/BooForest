using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringMovement))]
public class SteeringBehavior : MonoBehaviour
{

    public Camera _cam;
    private SteeringMovement _movement;

    [SerializeField()]
    public float seekWeight = 1.0f;

    // ------------- Arrive stuff ------------------------
    [SerializeField()]
    public float arriveWeight = 1.0f;
    public bool arriveOn { get; private set; }
    private Vector2 _arriveTarget = Vector2.zero;
    [HideInInspector()]
    public Vector2 targetPosition = Vector2.zero;
    public Decceleration defaultDecceleration = Decceleration.normal;


    // ------------- Wander stuff ------------------------
    [SerializeField()]
    private float   _wanderDistance = 5.0f;
    [SerializeField()]
    private float   _wanderRadius = 1.0f;
    [SerializeField()]
    private float   _wanderJitter = 0.01f; // max displacement from the circle per second
    private Vector2 _wanderTarget = Vector2.zero;
    [SerializeField()]
    private float   wanderWeight = 1.0f;
    public bool     wanderOn { get; set; } = false;

    // ------------- Pursuit stuff ----------------------
    private GameObject _pursuitTarget;
    public bool purseOn { get; private set; }

    public enum Decceleration
    {
        fast = 1,
        normal = 2,
        slow = 3
    };

    private void Awake()
    {
        // check cam
        if (_cam == null)
            Debug.LogError("MISSING CAMERA");

        // get movement and check if it was found
        _movement = GetComponent<SteeringMovement>();
        if (_movement == null)
            Debug.LogError("MISSING BASIC MOVEMENT COMPONENT");

        SetUpWander();
    }

    // Compute resulting Steering force
    public Vector2 Calculate()
    {
        Vector2 resultingForce = Vector2.zero;

        if (arriveOn) resultingForce += arriveWeight * Arrive(_arriveTarget, defaultDecceleration);
        if (wanderOn) resultingForce += wanderWeight * Wander();
        if (purseOn)  resultingForce += Pursuit(_pursuitTarget);

        return resultingForce;
    }

    // Seek steering behavior, go to a given position
    private Vector2 Seek(in Vector2 target)
    {
        Vector2 desiredVelocity = (target - (Vector2)transform.position).normalized * _movement.maxSpeed;

        return arriveWeight * (desiredVelocity - _movement.velocity);
    }

    // ---------------- ARRIVE --------------------
    private Vector2 Arrive(in Vector3 target, Decceleration decceleration = Decceleration.normal)
    {
        Vector2 toTarget = target - transform.position;
        var distance = toTarget.magnitude;

        if (distance <= 0.01) return Vector2.zero;

        const float tweaker = 0.003f;

        // Required speed
        float speed = distance / ((float)decceleration * tweaker);
        speed = Mathf.Min(speed, (float)_movement.maxSpeed);

        // normalize and scale to desired speed
        Vector2 desiredVelocity = toTarget * speed / distance;

        return desiredVelocity - _movement.velocity;
    }

    //      Arrive toward an specific position
    public void ArriveTo(in Vector2 target)
    {
        _arriveTarget = target;
        arriveOn = true;
    }
    //      Stop moving
    public void ArriveStop() => arriveOn = false;

    // ---------------- WANDER --------------------
    void SetUpWander()
    {
        _wanderTarget = _movement.heading * (_wanderDistance  + _wanderRadius);
    }

    private Vector2 Wander()
    {
        _wanderTarget.x += Random.Range(0.0f, 1.0f) * _wanderJitter;
        _wanderTarget.y += Random.Range(0.0f, 1.0f) * _wanderJitter;
        
        _wanderTarget = _wanderTarget.normalized * _wanderRadius;
        var targetLocal = _wanderTarget + Vector2.right * _wanderDistance;

            
        var targetWorld = new Vector2(
                                Vector2.Dot(targetLocal, _movement.heading),
                                Vector2.Dot(targetLocal, _movement.side)
                            ) + (Vector2) transform.position;

        Debug.DrawLine(transform.position, targetWorld, Color.green);

        return targetWorld - (Vector2) transform.position;
    }

    // ---------------- Pursue ---------------------

    public Vector2 Pursuit(in GameObject target)
    {
        // Check if target is valid
        if (target == null)
        {
            Debug.LogError("GIVEN TARGET TO PURSE IS NULL");
            return Vector2.zero;
        }
        SteeringMovement targetMovement = target.GetComponent<SteeringMovement>();
        if (targetMovement == null)
        {
            Debug.LogError("NO PUEDO PERSEGUIR A UN OBJETO SIN STEERING MOVEMENT");
            return Vector2.zero;
        }

        Vector2 toTarget = target.transform.position - transform.position;
        float relativeHeading = Vector2.Dot(_movement.heading, targetMovement.heading);

        if (Vector2.Dot(_movement.heading, toTarget) > 0 && relativeHeading < -0.95)
            return Seek(target.transform.position);

        float lookAheadTime = toTarget.magnitude / (_movement.maxSpeed + targetMovement.velocity.magnitude);

        var x = Seek((Vector2)target.transform.position + _movement.velocity * lookAheadTime);
        Debug.Log("Im pursuing from steering behavior " + x);
        return x;
    }

    public void Purse(in GameObject target)
    {
        _pursuitTarget = target;
        purseOn = true;
    }

    public void StopPurse() => purseOn = false;
}
