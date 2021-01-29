using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovement : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private bool _queued = false;

    protected Vector2 _velocity = Vector2.zero;
    public  Vector2 velocity { get { return _velocity; } }

    protected virtual void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();    
    }

    public void Move(Vector2 vel)
    {
        _queued = true;
        _velocity = vel;
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
