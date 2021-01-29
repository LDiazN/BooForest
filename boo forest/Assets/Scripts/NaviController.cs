
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

    public float naviSpeed = 1.0f;

    private Vector3 worldMousePos;

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
        Vector2 direction = worldMousePos - transform.position;

        transform.Translate(Time.deltaTime * direction * naviSpeed);
    }
}
