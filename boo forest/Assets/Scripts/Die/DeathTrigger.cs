using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerDeath;

    private void Die()
    {
        OnPlayerDeath.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Die"))
        {
            Debug.Log("Dying by plant");
            Die();
        }
    }
}
