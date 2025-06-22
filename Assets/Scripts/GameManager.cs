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
    public GameObject gameplayUI;

    [Header("Game Systems")]
    public RoundTimer roundTimer;
    public StressManager stressManager;

    [Header("Fade Settings")]
    public float fadeDuration = 0.3f;

    private float startRealTime;
    private bool isGameOver = false;

  void Start()
{
    startRealTime = 0f;
    isGameOver = false;
    
    gameOverScreen.SetActive(false);
    AudioManager.Instance.PlayRoundMusic();

    if (stressManager != null)
        stressManager.OnStressMaxed += () => GameOver(GameOverCause.Stress);

    if (roundTimer != null)
        roundTimer.OnTimeOutEvent += () => GameOver(GameOverCause.Timeout);
}

    void Update()
    {
        // Start timing after tutorial ends
    if (startRealTime == 0f && !TutorialManager.IsTutorialActive)
    {
        startRealTime = Time.realtimeSinceStartup;
        Debug.Log($"[TimerStart] startRealTime set to {startRealTime}");
    }

        // Debug fast-forward toggle
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = (Time.timeScale == 1f) ? 5f : 1f;
            Debug.Log("Time Scale: " + Time.timeScale);
        }

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

public void GameOver(GameOverCause cause)
{
    if (isGameOver) return; // Prevent double-call
    isGameOver = true;

    roundTimer?.PauseTimer();

    // Store final time
    float survivalTime = Time.realtimeSinceStartup - startRealTime;

    float bestTime = PlayerPrefs.GetFloat(BestTimeKey, 0f);
    bool isNewRecord = survivalTime > bestTime;
    if (isNewRecord)
    {
        PlayerPrefs.SetFloat(BestTimeKey, survivalTime);
        PlayerPrefs.Save();
    }

    string reasonText = cause == GameOverCause.Stress
        ? "Granny got too stressed out!"
        : "YOU'RE TAKING TOO LONG";

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

    StartCoroutine(ShowGameOverUI());
}

IEnumerator ShowGameOverUI()
{
    // ✅ Hide all gameplay UI
    if (gameplayUI != null)
        gameplayUI.SetActive(false);

    gameOverScreen.SetActive(true);

    // 🔊 Play SFX
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayGameOverSFX();
    }

    yield return new WaitForSecondsRealtime(0.5f);

    // 🛑 Freeze the game
    Time.timeScale = 0f;
}


}
