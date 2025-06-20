using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float maxTime = 120f;
    public TMP_Text timerText;

    private float timePassed = 0f;

    public float TimePassed => timePassed;

    void Update()
    {
        timePassed += Time.deltaTime;

        float timeLeft = Mathf.Max(0f, maxTime - timePassed);
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
