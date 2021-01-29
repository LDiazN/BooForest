using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicMovement))]
public class SteeringBehavior : MonoBehaviour
{

    public Camera _cam;
    private BasicMovement _movement;

    [SerializeField()]
    public float seekWeight = 1.0f;

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
        _movement = GetComponent<BasicMovement>();
        if (_movement == null)
            Debug.LogError("MISSING BASIC MOVEMENT COMPONENT");
    }

    // Compute resulting Steering force
    public Vector2 Calculate()
    {
        return Arrive((Vector2) (_cam.ScreenToWorldPoint(Input.mousePosition)));
    }

    // Seek steering behavior, go to a given position
    private Vector2 Seek(in Vector2 target)
    {
        Vector2 desiredVelocity = (target - (Vector2) transform.position).normalized * _movement.maxSpeed;
         
        return seekWeight * (desiredVelocity - _movement.velocity);
    }

    private Vector2 Arrive(in Vector3 target, Decceleration decceleration = Decceleration.normal)
    {
        Vector2 toTarget = target - transform.position;
        var distance = toTarget.magnitude;

        if (distance <= 0.01) return Vector2.zero;

        const float tweaker = 0.03f;

        // Required speed
        float speed = distance / ((float) decceleration * tweaker);
        speed = Mathf.Min(speed,(float) _movement.maxSpeed);

        // normalize and scale to desired speed
        Vector2 desiredVelocity = toTarget * speed / distance;

        return desiredVelocity - _movement.velocity;
    }
}
