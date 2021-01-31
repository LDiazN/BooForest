using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Transform _navi;

    private Camera _cam;

    private Vector3 _followVel = Vector3.zero;
    private float _sizeVel = 0f;

    [SerializeField]
    private float _minDistance = 5f;
    [SerializeField]
    private float _minSize = 4f;

    [SerializeField]
    private float _padding = 4f;

    private void Awake()
    {
        _cam = GetComponent<Camera>();    
    }


    private void LateUpdate()
    {
        Vector2 playerPos = _player.transform.position;
        Vector2 naviPos = _navi.transform.position;
        Vector3 targetPos = Vector3.Lerp(playerPos, naviPos, 0.5f);
        targetPos.z = transform.position.z;

        float vertDist = Mathf.Abs(naviPos.y - playerPos.y);
        float horDist = Mathf.Abs(naviPos.x - playerPos.x);
        float targetSize = 0f;

        if (vertDist > horDist)
        {
            //Debug.Log("Vert");
            targetSize = vertDist / 2f;
            targetSize += _padding;
        }
        else
        {
            //Debug.Log("Hor");
            targetSize = horDist * ((float)Screen.height / (float)Screen.width) * 0.5f;
            targetSize += _padding;
        }
        

        //Debug.Log($"HORIZONTAL: {horDist} VERTICAL: {vertDist}");
        //Debug.Log($"TARGET SIZE: {targetSize}");
        _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, targetSize, ref _sizeVel, 0.2f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _followVel, 0.2f);
    }
}
