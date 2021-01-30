using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnPlayerDeath;

    private void Die()
    {
        OnPlayerDeath.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }
}
