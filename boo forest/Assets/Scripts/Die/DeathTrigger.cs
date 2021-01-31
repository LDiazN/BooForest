using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerDeath;

    private AudioSource _deathSrc;

    private void Awake()
    {
        if (_deathSrc == null)
            Debug.LogWarning("Necesitas un audio source para el death trigger");
        _deathSrc.loop = false;
    }


    private void Die()
    {
        _deathSrc.Play();
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
