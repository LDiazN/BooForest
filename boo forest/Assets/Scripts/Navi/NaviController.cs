
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviController : MonoBehaviour
{

    [SerializeField()]
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

    // Insertar componente de mover player

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

    
    private void SignalPos()
    {
        worldMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        target = worldMousePos;
        _turboing = true;
    }
}
