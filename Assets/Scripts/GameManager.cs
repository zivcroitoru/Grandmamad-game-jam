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
        startTime = Time.time;
        gameOverScreen.SetActive(false);

        // Hook into events
        roundTimer.OnTimeOutEvent += () => TriggerGameOver(GameOverCause.Timeout);
        stressManager.OnStressMaxed += () => TriggerGameOver(GameOverCause.Stress);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        // Fast forward toggle for debug
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = (Time.timeScale == 1f) ? 5f : 1f;
            Debug.Log($"⏩ Fast Forward: {(Time.timeScale > 1f ? "ON" : "OFF")}");
        }

        // Restart
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void TriggerGameOver(GameOverCause reason)
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        if (timerUI != null) timerUI.SetActive(false);
        if (stressUI != null) stressUI.SetActive(false);

        float survivalTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);

        float bestTime = PlayerPrefs.GetFloat(BestTimeKey, 0f);
        bool isNewRecord = survivalTime >= bestTime;

        if (isNewRecord)
        {
            PlayerPrefs.SetFloat(BestTimeKey, survivalTime);
            PlayerPrefs.Save();
        }

        int bestMin = Mathf.FloorToInt(bestTime / 60f);
        int bestSec = Mathf.FloorToInt(bestTime % 60f);

        string reasonText = reason == GameOverCause.Stress
            ? "Granny couldn't take the pressure..."
            : "YOUR TAKING TOO LONG";

        string timeText = $"{minutes:00}:{seconds:00}";
        string bestTimeTextValue = $"Best: {bestMin:00}:{bestSec:00}";

        string styledText =
        "<size=125><b>Game Over</b></size>\n\n" +
        $"<size=24>{reasonText}</size>\n\n" +
        $"<size=75><b>{timeText}</b></size>\n\n" +
        $"<size=32>{bestTimeTextValue}</size>\n\n";

    if (isNewRecord)
        styledText += "<size=18><color=#FFD700>🏆 New Record!</color></size>\n\n";

    styledText += "\n<size=24><color=#FFFFFFAA>Press R to Restart  |  Esc to Quit</color></size>";


        gameOverText.text = styledText;

        StartCoroutine(FadeInGameOverScreen());
    }

    IEnumerator FadeInGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        gameOverCanvasGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
