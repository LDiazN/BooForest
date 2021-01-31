using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SteeringBehavior))]
public class PlayerController : MonoBehaviour
{

    Action CurrentAction;

    private Vector2 _goToTarget;
    private SteeringBehavior _behaviorMachine;

    // Start is called before the first frame update
    void Start()
    {
        // Init movement state as idle
        CurrentAction = Idle;

        // Init SteeringBehavior
        _behaviorMachine = GetComponent<SteeringBehavior>();
        if (_behaviorMachine == null)
            Debug.LogError("MISSING STEERING BEHAVIOR");
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAction();
    }

    public void GoTo(in Vector2 target)
    {
        _goToTarget = target;
        CurrentAction = ArrivePosition;
    }

    public void Stop()
    {
        _behaviorMachine.ArriveStop();
        CurrentAction = Idle;
    }

    // when the player is doing nothing
    private void Idle()
    {
    }

    private void ArrivePosition()
    {

        _behaviorMachine.ArriveTo(_goToTarget);
        var toTarget = _goToTarget - (Vector2)transform.position;

        if (toTarget.sqrMagnitude <= 0.01)
        {
            Stop();
        }
    }
}
