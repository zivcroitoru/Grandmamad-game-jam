using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public float maxTime = 61f;
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

    int seconds = Mathf.FloorToInt(timeLeft % 61f);

    if (roundTimerText != null)
        roundTimerText.text = $"{seconds:00}";

    if (timeLeft <= 0f)
    {
        timedOut = true;
        OnTimeOutEvent?.Invoke();
    }
}


    public void ResetTimer(float newMaxTime = 61f)
    {
        maxTime = newMaxTime;
        timePassed = 0f;
        timedOut = false;
    }
}
