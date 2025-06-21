using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public float maxTime = 60f;
    public TMP_Text roundTimerText;
    private bool isPaused = false;

    private float timePassed = 0f;
    private bool timedOut = false;

    public System.Action OnTimeOutEvent;
    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

  void Update()
{
    if (isPaused || timedOut) return;

    timePassed += Time.deltaTime;
    float timeLeft = Mathf.Max(0f, maxTime - timePassed);

    int minutes = Mathf.FloorToInt(timeLeft / 60f);
    int seconds = Mathf.FloorToInt(timeLeft % 60f);

    if (roundTimerText != null)
        roundTimerText.text = $"{minutes:00}:{seconds:00}";

    if (timeLeft <= 0f)
    {
        timedOut = true;
        OnTimeOutEvent?.Invoke();
    }
}


    public void ResetTimer(float newMaxTime = 60f)
    {
        maxTime = newMaxTime;
        timePassed = 0f;
        timedOut = false;
    }
}
