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

    [SerializeField()]
    public float arriveWeight = 1.0f;
    public bool  arriveOn { get; private set; }
    private Vector2 _arriveTarget = Vector2.zero;

    [HideInInspector()]
    public Vector2 targetPosition = Vector2.zero;

    public Decceleration defaultDecceleration = Decceleration.normal;

    public enum Decceleration
    {
        fast    = 1,
        normal  = 2,
        slow    = 3
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
    }

    // Compute resulting Steering force
    public Vector2 Calculate()
    {
        Vector2 resultingForce = Vector2.zero;

        if (arriveOn) resultingForce += arriveWeight * Arrive(_arriveTarget, defaultDecceleration);

        return resultingForce;
    }

    // Seek steering behavior, go to a given position
    private Vector2 Seek(in Vector2 target)
    {
        Vector2 desiredVelocity = (target - (Vector2) transform.position).normalized * _movement.maxSpeed;
         
        return arriveWeight * (desiredVelocity - _movement.velocity);
    }

    private Vector2 Arrive(in Vector3 target, Decceleration decceleration = Decceleration.normal)
    {
        Vector2 toTarget = target - transform.position;
        var distance = toTarget.magnitude;

        if (distance <= 0.01) return Vector2.zero;

        const float tweaker = 0.003f;

        // Required speed
        float speed = distance / ((float) decceleration * tweaker);
        speed = Mathf.Min(speed, (float) _movement.maxSpeed);

        // normalize and scale to desired speed
        Vector2 desiredVelocity = toTarget * speed / distance;

        return desiredVelocity - _movement.velocity;
    }

    public void ArriveTo(in Vector2 target)
    {
        _arriveTarget = target;
        arriveOn = true;
    }

    public void ArriveStop() => arriveOn = false;
}
