using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(CircleCollider2D), typeof(Light2D))]
public class Lamp : MonoBehaviour
{
    private Light2D _light2D;
    private CircleCollider2D _coll2d;
    
    [SerializeField]
    private Color _color;

    [SerializeField]
    private float _radius = 6f;
    [SerializeField]
    private float _offTime = 3f;

    private bool _off = false;

    private void Awake()
    {
        _light2D = GetComponent<Light2D>();
        _light2D.lightType = Light2D.LightType.Point;
        _light2D.pointLightOuterRadius = _radius;
        _light2D.color = _color;

        _coll2d = GetComponent<CircleCollider2D>();
        _coll2d.isTrigger = true;
        _coll2d.radius = _radius;
    }


    public IEnumerator TurnOffLight()
    {
        _off = true;
        float initialInt = _light2D.intensity;
        float step = 1f / _offTime;
        float t = 0f;
  
        while(_light2D.intensity != 0)
        {
            t += step * Time.deltaTime;
            _light2D.intensity = Mathf.Lerp(initialInt, 0f, t);
            _coll2d.radius = Mathf.Lerp(_radius, 0f, t);
            yield return null;
        }

        _coll2d.enabled = false;
        _light2D.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_off)
            return;

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TurnOffLight());
        }
    }
}
