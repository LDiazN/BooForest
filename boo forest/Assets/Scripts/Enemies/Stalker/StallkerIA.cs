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

        CurrentAction = Avoid;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAction();
    }

    private void Wandering()
    {
        _behaviorMachine.wanderOn = true;
    }

    private void Pursuing()
    {
        var player = PlayerStatus.Instance.gameObject;
        _behaviorMachine.Purse(player);

        Vector2 toPlayer = player.transform.position - transform.position;
        if (toPlayer.sqrMagnitude < 0.001)
        {
            _behaviorMachine.StopPurse();
            CurrentAction = Idle;
        }
    }

    private void Avoid()
    {
        var player = PlayerStatus.Instance.gameObject;
        Vector2 toPlayer = player.transform.position - transform.position;
        if (toPlayer.sqrMagnitude < 4 * 4)
            _behaviorMachine.EvadeTo(player);
        else
            _behaviorMachine.StopEvade();

    }

    private void Idle()
    {

    }
}
