using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    public static UIManagement Instance { get; private set; }

    public GameObject gameOverUI;
    public GameObject endLevelUI;
    public Image blackImage;

    private CanvasGroup gameOverCG;
    private CanvasGroup endLevelCG;

    public float uiFadeTime = 2.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Mira maldito, esto es un singleton ." + gameObject.name);
        }
        Instance = this;

        gameOverCG = gameOverUI.GetComponent<CanvasGroup>();
        endLevelCG = endLevelUI.GetComponent<CanvasGroup>();

        gameOverCG.alpha = 0f;
        endLevelCG.alpha = 0f;

        gameOverUI.SetActive(false);
        endLevelUI.SetActive(false);

        blackImage.color = new Color(0f, 0f, 0f, 1f);
    }

    private void Start()
    {
        GameStatus.Instance.OnGameOver.AddListener(EnableGOUI);
        GameStatus.Instance.OnVictory.AddListener(EnableELUI);
    }

    public void EnableGOUI()
    {
        gameOverUI.SetActive(true);
        StartCoroutine(FadeInUI(gameOverCG));
    }


    public void EnableELUI()
    {
        endLevelUI.SetActive(true);
        StartCoroutine(FadeInUI(endLevelCG));
    }


    private IEnumerator FadeInUI(CanvasGroup cg)
    {
        float step = 1f / uiFadeTime;
        float t = 0f;

        while (cg.alpha != 1f)
        {
            t += step * Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1f, t);
            yield return null;
        }

        cg.alpha = 1f;
    }

    public IEnumerator FadeScreen(float fadeTime, bool toZero)
    {
        float step = 1f / fadeTime;
        float t = 0f;
        float val = 1f;

        float a = (toZero ? 1f : 0f);
        float b = (toZero ? 0f : 1f);

        while (blackImage.color.a != 0f)
        {
            t += step * Time.deltaTime;
            val = Mathf.Lerp(a, b, t);
            blackImage.color = new Color(0f, 0f, 0f, val);
            Debug.Log(val);
            yield return null;
        }

        blackImage.color = new Color(0f, 0f, 0f, 0f);
    }
}
