using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SteeringBehavior))]
public class StallkerIA : MonoBehaviour
{

    SteeringBehavior _behaviorMachine;
    Action CurrentAction;

    private void Awake()
    {
        _behaviorMachine = GetComponent<SteeringBehavior>();
        if (_behaviorMachine == null)
            Debug.LogError("MISSING STEERING MOVEMENT COMPONENT IN STALKER");

        CurrentAction = Pursuing;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAction();
    }

    void Wandering()
    {
        _behaviorMachine.wanderOn = true;
    }

    void Pursuing()
    {
        var player = PlayerStatus.Instance.gameObject;
        _behaviorMachine.Pursuit(player);
    }
}
