
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class NaviController : MonoBehaviour
{
    private Light2D _light;
    [SerializeField]
    private Color _color = Color.white;

    [SerializeField]
    private Camera _cam;

    [SerializeField()]
    public GameObject player;

    public KeyCode Call;

    [SerializeField]
    private float _maxSpeed = 10f;
    [SerializeField]
    private float _smoothTime = 0.1f;

    private bool _turboing = false;
    [SerializeField]
    private float _turboMaxSpeed = 200f;
    [SerializeField]
    private float _turboSmoothTime = 0.05f;

    private Vector3 worldMousePos;
    private Vector2 target = Vector2.zero;
    private Vector2 _vel = Vector2.zero;

    [SerializeField]
    private float _radius = 10f;
    [SerializeField]
    private LayerMask _signalLayer = 0;

    // Insertar componente de mover player


    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _light.lightType = Light2D.LightType.Point;
        _light.pointLightOuterRadius = _radius / 4f;
        _light.color = _color;
    }


    // Start is called before the first frame update
    private void Start()
    {
        if (player == null)
            Debug.Log("EL PLAYER ES NULL");
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(Call))
        {
            SignalPos();
            Debug.Log("Targeting to position " + worldMousePos);
        }

        GoingPosition(_turboing);

        if (_turboing)
        {
            if (Mathf.Approximately(Vector2.SqrMagnitude(target - (Vector2)transform.position), 0))
                _turboing = false;
        }
    }


    // Go to mouse position
    private void GoingPosition(bool turbo = false)
    {
        worldMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (!turbo)
            transform.position = Vector2.SmoothDamp(transform.position, worldMousePos, ref _vel, _smoothTime, _maxSpeed);
        else if (turbo)
            transform.position = Vector2.SmoothDamp(transform.position, target, ref _vel, _turboSmoothTime, _turboMaxSpeed);
    }


    private void ProcessSignalHits(Collider2D[] hits)
    {
        // Code that processes signal hits goes here!
    }

    
    private void SignalPos()
    {
        worldMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        target = worldMousePos;
        _turboing = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(target, _radius, _signalLayer);
        ProcessSignalHits(hits);
    }
}
