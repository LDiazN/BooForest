using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SteeringMovement _behaviour;

    private bool left = false;
    private bool right = false;
    private bool up = false;
    private bool down = false;


    private void Update()
    {
        Vector3 vel = _behaviour.velocity;

        if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
        {
            if (!left && vel.x < 0)
            {
                left = true;
                right = up = down = false;
                _animator.Play("Left");
            }
            else if (!right && vel.x > 0)
            {
                right = true;
                left = up = down = false;
                _animator.Play("Right");
            }
        }
        else
        {
            if (!up && vel.y > 0)
            {
                up = true;
                right = down = left = false;
                _animator.Play("Back");
            }
            else if (!down && vel.y < 0)
            {
                down = true;
                right = left = up = false;
                _animator.Play("Front");
            }
        }
    }
}
