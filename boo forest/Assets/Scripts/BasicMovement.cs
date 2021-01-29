using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovement : MonoBehaviour
{
    private Rigidbody2D _rb2d;

    private Vector2 _vel = Vector2.zero;
    private bool _queued = false;


    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();    
    }


    public void Move(Vector2 vel)
    {
        _queued = true;
        _vel = vel;
    }


    private void FixedUpdate()
    {
        if (_queued)
        {
            _rb2d.MovePosition((Vector2)transform.position + _vel * Time.fixedDeltaTime);
            _queued = false;
            _vel = Vector2.zero;
        }
    }
}
