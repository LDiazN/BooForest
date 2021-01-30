using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class EndLevelTrigger : MonoBehaviour
{
    public int nextLevel;

    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GameStatus.Instance.OnVictory.Invoke();
    }
}
