
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

    private Vector3 worldMousePos;
    private Vector2 _vel = Vector2.zero; 

    // Insertar componente de mover player

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.Log("EL PLAYER ES NULL");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Call))
            Debug.Log("Moving to position " + worldMousePos);

        GoingPosition();
    }


    // Go to mouse position
    void GoingPosition()
    {
        worldMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.SmoothDamp(transform.position, worldMousePos, ref _vel, _smoothTime, _maxSpeed);
    }
}
