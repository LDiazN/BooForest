using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PositionEvent : UnityEvent<Vector2> { }

[RequireComponent(typeof(CircleCollider2D))]
public class KillerPlantIA : MonoBehaviour
{
    [SerializeField()]
    public PositionEvent OnSignalStarted;

    [SerializeField()]
    private float _SpawnRadius;

    [SerializeField()]
    private int _MaxSimoultaneusSignals = 1;

    [SerializeField()]
    private float _minTimeBetweenSignals = 2.0f;
    [SerializeField()]
    private float _maxTimeBetweenSignals = 3.0f;

    [SerializeField()]
    private float _maxAttractionRadiusPerSignal = 1.0f;

    private CircleCollider2D circleArea;

    private void Awake()
    {
        circleArea = GetComponent<CircleCollider2D>();
        if (circleArea == null)
        {
            Debug.LogError("ERROR IN KILLER PLANT: I SHOULD HAVE A CIRCLE COLLIDER 2D");
            return;
        }

        circleArea.isTrigger = true;
    }

    private void Start()
    {
        for (var i = 0; i < _MaxSimoultaneusSignals; i++)
            StartCoroutine(SignalSpawner());
    }

    private void SpawnSignal()
    {

        float distanceToPlayer = (transform.position - PlayerStatus.Instance.transform.position).magnitude;
        float radius = Mathf.Min(distanceToPlayer, _SpawnRadius);

        Vector2 position = new Vector2(
                Random.Range(0.0f, radius),
                Random.Range(0.0f, radius)
            );

        if (position.sqrMagnitude > radius*radius)
            position = position.normalized * radius;

        position += (Vector2) transform.position;

        Vector2 toPlayer = (Vector2) PlayerStatus.Instance.transform.position - position;
        if (toPlayer.sqrMagnitude < _maxAttractionRadiusPerSignal)
        {
            PlayerController controller = PlayerStatus.Instance.GetComponent<PlayerController>();
            if (controller == null)
            {
                OnSignalStarted.Invoke(position);
                Debug.LogError("ERROR IN KILLER PLANT: PLAYER SHOULD HAVE A PLAYER CONTROLLER COMPONENT");
                return;
            }

            controller.GoTo(position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _SpawnRadius);
    }

    IEnumerator SignalSpawner()
    {
        float time = 0.0f;
        float maxTime = Random.Range(_minTimeBetweenSignals, _maxTimeBetweenSignals);

        while (true)
        {
            time += Time.fixedDeltaTime;
            if (time >= maxTime)
            {
                SpawnSignal();
                time = 0.0f;
                maxTime = Random.Range(_minTimeBetweenSignals, _maxTimeBetweenSignals);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
