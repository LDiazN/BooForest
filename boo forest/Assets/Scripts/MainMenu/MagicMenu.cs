using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicMenu : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject creditUI;

    public AudioSource src;

    private void Awake()
    {
        src.loop = false;
        creditUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credit()
    {
        mainUI.SetActive(false);
        creditUI.SetActive(true);
    }

    public void BackMenu()
    {
        mainUI.SetActive(true);
        creditUI.SetActive(false);
    }

    public void MoarKittySounds()
    {
        if (src.isPlaying)
            return;

        src.Play();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
