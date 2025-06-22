using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameOverCause
{
    Stress,
    Timeout
}

public class GameManager : MonoBehaviour
{
    private const string BestTimeKey = "BestSurvivalTime";

    [Header("UI References")]
    public GameObject timerUI;
    public GameObject stressUI;
    public GameObject gameOverScreen;
    public TMP_Text gameOverText;
    public CanvasGroup gameOverCanvasGroup;

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;

    [Header("Game Systems")]
    public RoundTimer roundTimer;
    public StressManager stressManager;

    private float startTime;
    private bool isGameOver = false;

    void Start()
    {
        gameOverScreen.SetActive(false);
        StartCoroutine(DelayedGameStart());
        AudioManager.Instance.PlayRoundMusic();
    }

    IEnumerator DelayedGameStart()
    {
        yield return new WaitUntil(() => !TutorialManager.IsTutorialActive);
        startTime = Time.time;
    }

    public void GameOver(GameOverCause cause)
    {
        if (isGameOver) return;
        isGameOver = true;

        float survivalTime = Time.time - startTime;
        float bestTime = PlayerPrefs.GetFloat(BestTimeKey, 0f);

        bool isNewRecord = survivalTime > bestTime;
        if (isNewRecord)
        {
            PlayerPrefs.SetFloat(BestTimeKey, survivalTime);
            PlayerPrefs.Save();
        }

        string reasonText = cause == GameOverCause.Stress
            ? "Granny got too stressed out!"
            : "Time ran out!";

        string timeText = $"{survivalTime:F1} seconds";
        string bestTimeTextValue = $"Best Time: {Mathf.Max(survivalTime, bestTime):F1} sec";

        string styledText =
            $"<size=94><b>Game Over</b></size>\n\n" +
            $"<size=18>{reasonText}</size>\n\n" +
            $"<size=56><b>{timeText}</b></size>\n\n" +
            $"<size=24>{bestTimeTextValue}</size>\n\n";

        if (isNewRecord)
            styledText += "<size=18><color=#FFD700><b>🏆 New Record!</b></color></size>\n\n";

        styledText += Application.isMobilePlatform
            ? "\n<size=18><color=#FFFFFFAA>Tap the screen to restart</color></size>"
            : "\n<size=18><color=#FFFFFFAA>Press R to Restart  |  Q to Quit</color></size>";

        gameOverText.text = styledText;

        StartCoroutine(FadeInGameOverScreen());
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSFX);
    }

    IEnumerator FadeInGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        gameOverCanvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            gameOverCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
    }

    void Update()
    {
        if (!isGameOver) return;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else if (Input.GetKeyDown(KeyCode.Q))
                Application.Quit();
        }
    }
}
