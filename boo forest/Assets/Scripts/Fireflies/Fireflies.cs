using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D), typeof(Animator))]
public class Fireflies : MonoBehaviour
{
    private Light2D _light;

    private Animator _animator;
    [SerializeField]
    private AnimationClip _anim;

    [SerializeField]
    private Vector2 _center = Vector2.zero;
    [SerializeField]
    private float _radius = 4f;

    [SerializeField]
    private float _cd = 4f;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _animator = GetComponent<Animator>();

        _center = transform.position;
    }


    private void Start()
    {
        StartFirefly();
    }


    public void StartFirefly()
    {
        StartCoroutine(MainRoutine());
    }


    public void DisableFirefly()
    {
        StopAllCoroutines();
    }


    private IEnumerator MainRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(AppearFirefly());
            yield return new WaitForSeconds(_cd);
            transform.position = GetRandomPos(_center);
        }
    }


    private IEnumerator AppearFirefly()
    {
        _animator.Play("TurnOnFirefly");
        yield return new WaitForSeconds(_anim.length);
    }


    private Vector2 GetRandomPos(Vector2 center)
    {
        return center + Random.insideUnitCircle * _radius;
    }


    private void OnDrawGizmosSelected()
    {
        if (_radius > 0)
            Gizmos.DrawWireSphere(_center, _radius);
    }
}
