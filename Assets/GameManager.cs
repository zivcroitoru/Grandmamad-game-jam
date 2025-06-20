using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameOverCause
{
    Stress,
    Timeout
}

public class GameManager : MonoBehaviour
{
    private const string BestTimeKey = "BestSurvivalTime";

    public RoundTimer roundTimer;
    public StressManager stressManager;

    public GameObject gameOverScreen;
    public TMP_Text gameOverText;

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
        gameOverScreen.SetActive(true);

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
            : "She didn't reach the Mamad in time.";

        string timeText = $"{minutes:00}:{seconds:00}";
        string bestTimeTextValue = $"Best: {bestMin:00}:{bestSec:00}";

        // Build stylized TMP text
string styledText =
    "<size=250><b>Game Over</b></size>\n\n" +
    $"<size=48><i>{reasonText}</i></size>\n\n" +
    $"<size=150><b>{timeText}</b></size>\n\n" +
    $"<size=36>{bestTimeTextValue}</size>\n\n";

if (isNewRecord)
    styledText += "<size=36><color=#FFD700>🏆 New Record!</color></size>\n\n";

// ⬇️ Added blank line before this final line
styledText += "\n<size=32><color=#FFFFFFAA>Press R to Restart  |  Esc to Quit</color></size>";


    }
public void QuitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // for testing in editor
#else
    Application.Quit(); // for builds
#endif
}
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
