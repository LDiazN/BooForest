using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SteeringBehavior))]
public class StallkerIA : MonoBehaviour
{

    [SerializeField()]
    private float _detectingRadius = 3.0f;
    [SerializeField()]
    private float _criticalRadius = 1.0f;
    [SerializeField()]
    private float _criticalSpeed = 10000.0f;
    [SerializeField()]
    private float _minimumStalkSpeed = 10.0f;
    [SerializeField()]
    private float _maximumStalkSpeed = 100.0f;
    [SerializeField()]
    private float _distanceFromCriticalRadius = 4;
    [SerializeField()]
    private float _maxDistanceToAvoid = 3;

    SteeringBehavior _behaviorMachine;
    Action CurrentAction;

    private SteeringMovement _movement;

    private enum GizmoState
    {
        stalk,
        wander,
        avoid,
        none,
        all
    }
    private GizmoState _currentGizmoState = GizmoState.all;

    private void Awake()
    {
        _behaviorMachine = GetComponent<SteeringBehavior>();
        if (_behaviorMachine == null)
            Debug.LogError("MISSING STEERING BEHAVIOR COMPONENT IN STALKER");

        _movement = GetComponent<SteeringMovement>();
        if (_movement == null)
            Debug.LogError("MISSING STEERING MOVEMENT COMPONENT IN STALKER");

        CurrentAction = Wandering;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAction();
    }

    private void Wandering()
    {
        _currentGizmoState = GizmoState.wander;
        var player = PlayerStatus.Instance;
        _behaviorMachine.wanderOn = true;


        Vector2 toPlayer = player.transform.position - transform.position;
        if (toPlayer.sqrMagnitude <= _detectingRadius* _detectingRadius)
            CurrentAction = Stalk;

    }

    private void Stalk()
    {
        _currentGizmoState = GizmoState.stalk;
        var player = PlayerStatus.Instance.gameObject;

        // Check if the player is safe in the light
        if (PlayerStatus.Instance.inLight)
        {
            Debug.Log("OH NO PLAYER IS SAFE");
            CurrentAction = Avoid;
            return;
        }
        

        _behaviorMachine.Purse(player);
        Vector2 toPlayer = player.transform.position - transform.position;

        // Setup speed
        if (toPlayer.sqrMagnitude <= _criticalRadius * _criticalRadius)
            _movement.maxSpeed = _criticalSpeed;
        else
        {
            var t = (toPlayer.magnitude - _criticalRadius) / _distanceFromCriticalRadius;
            _movement.maxSpeed = Mathf.Lerp(_maximumStalkSpeed, _minimumStalkSpeed, t);
        }

        if (toPlayer.sqrMagnitude < 0.01)
        {
            _behaviorMachine.StopPurse();
            CurrentAction = Idle;
        }
    }

    private void Avoid()
    {
        // If player is not safe, stalk him
        if (!PlayerStatus.Instance.inLight)
        {
            CurrentAction = Stalk;
            return;
        }
        // Update gizmo state
        _currentGizmoState = GizmoState.avoid;

        // Avoid
        var player = PlayerStatus.Instance.gameObject;
        Vector2 toPlayer = player.transform.position - transform.position;

        if (toPlayer.sqrMagnitude < _maxDistanceToAvoid * _maxDistanceToAvoid)
            _behaviorMachine.EvadeTo(player);
        else
            _behaviorMachine.StopEvade();


    }

    private void Idle()
    {
        _currentGizmoState = GizmoState.none;
        _movement.Stop();
    }

    private void OnDrawGizmos()
    {
        if (_currentGizmoState == GizmoState.none) return;

        if (_currentGizmoState == GizmoState.wander || _currentGizmoState==GizmoState.all)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _detectingRadius);
        }

        if (_currentGizmoState == GizmoState.stalk || _currentGizmoState == GizmoState.all)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _criticalRadius);
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, _criticalRadius + _distanceFromCriticalRadius);
        }
        if((PlayerStatus.Instance != null) && (_currentGizmoState == GizmoState.avoid || _currentGizmoState == GizmoState.all))
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(PlayerStatus.Instance.transform.position, _maxDistanceToAvoid);
        }
    }
}
