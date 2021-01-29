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

    private void Awake()
    {
        if (_cam == null)
            Debug.LogError("MISSING CAMERA");

        _movement = GetComponent<BasicMovement>();
        if (_movement == null)
            Debug.LogError("MISSING BASSIC MOVEMENT COMPONENT");
    }

    // Compute resulting Steering force
    public Vector2 Calculate()
    {

        return Seek((Vector2) (_cam.ScreenToWorldPoint(Input.mousePosition)));
    }

    // Seek steering behavior, go to a given position
    private Vector2 Seek(in Vector2 target)
    {
        Vector2 desiredVelocity = (target - (Vector2) transform.position).normalized * _movement.maxSpeed;
         
        return seekWeight * (desiredVelocity - _movement.velocity);
    }
    
}
