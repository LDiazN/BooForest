using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject endLevelUI;

    private CanvasGroup gameOverCG;
    private CanvasGroup endLevelCG;

    public float fadeTime = 2.5f;

    private void Awake()
    {
        gameOverCG = gameOverUI.GetComponent<CanvasGroup>();
        endLevelCG = endLevelUI.GetComponent<CanvasGroup>();

        gameOverCG.alpha = 0f;
        endLevelCG.alpha = 0f;

        gameOverUI.SetActive(false);
        endLevelUI.SetActive(false);
    }

    private void Start()
    {
        GameStatus.Instance.OnGameOver.AddListener(EnableGOUI);
        GameStatus.Instance.OnVictory.AddListener(EnableELUI);
    }


    public void EnableGOUI()
    {
        gameOverUI.SetActive(true);
        FadeInUI(gameOverCG);
    }


    public void EnableELUI()
    {
        endLevelUI.SetActive(true);
        FadeInUI(endLevelCG);
    }


    private IEnumerator FadeInUI(CanvasGroup cg)
    {
        float step = 1f / 2.5f;
        float t = 0f;

        while (cg.alpha != 1f)
        {
            t += step;
            cg.alpha = Mathf.Lerp(0, 1f, t);
            yield return null;
        }

        cg.alpha = 1f;
    }
}
