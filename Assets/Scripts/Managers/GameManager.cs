using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameStartManager gameStartManager;
    public GameOverManager gameOverManager;

    public RoundTimer roundTimer;       // ⏱️ Handles countdown
    public RoundTracker roundTracker;   // 🔢 Tracks round count

    public StressManager stressManager;

    private bool isGameOver;

    void Start()
    {
        roundTracker.ResetRounds();
        isGameOver = false;

        stressManager.OnStressMaxed += () => TriggerGameOver(GameOverCause.Stress);
        roundTimer.OnTimeOutEvent += () => TriggerGameOver(GameOverCause.Timeout);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = (Time.timeScale == 1f) ? 4f : 1f;
            Debug.Log($"Time scale set to: {Time.timeScale}");
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.touchCount > 0)
            {
                RestartGame();
            }
        }
    }

    void TriggerGameOver(GameOverCause cause)
    {
        if (isGameOver) return;
        isGameOver = true;

        roundTimer?.PauseTimer();
        gameOverManager.ShowGameOver(cause, roundTracker.RoundsPassed);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
