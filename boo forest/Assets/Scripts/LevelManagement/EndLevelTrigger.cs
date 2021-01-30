using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class EndLevelTrigger : MonoBehaviour
{
    public int nextLevel;


    public void ToMainMenu()
    {
        StartCoroutine(LoadNextLevel(0));
    }


    public void LoadLevel(int level)
    {
        StartCoroutine(LoadNextLevel(level));
    }


    public void RestartLevel()
    {
        StartCoroutine(LoadNextLevel(SceneManager.GetActiveScene().buildIndex));
    }


    public IEnumerator LoadNextLevel(int level)
    {
        yield return UIManagement.Instance.StartCoroutine(
            UIManagement.Instance.FadeScreen(GameStatus.Instance.fadeInTime, false));

        SceneManager.LoadScene(level);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GameStatus.Instance.OnVictory.Invoke();
    }
}
