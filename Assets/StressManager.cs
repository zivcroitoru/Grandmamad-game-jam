using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StressManager : MonoBehaviour
{
    public float stress = 0f;
    public float maxStress = 100f;
    public float stressRate = 2f;

    public float firstAlarmTime = 30f;
    public float secondAlarmTime = 60f;

    public Slider stressBar;
    public TMP_Text stressText;

    public TimerManager timer; // Drag your TimerManager here

    void Update()
    {
        float t = timer.TimePassed;

        // Stress curve
        if (t > secondAlarmTime)
            stress += Time.deltaTime * (stressRate * 3f);
        else if (t > firstAlarmTime)
            stress += Time.deltaTime * (stressRate * 2f);
        else
            stress += Time.deltaTime * stressRate;

        // Clamp & UI
        stress = Mathf.Clamp(stress, 0f, maxStress);
        if (stressBar != null)
            stressBar.value = stress / maxStress;

        if (stressText != null)
            stressText.text = $"{(stress / maxStress * 100f):0}%";

        if (stress >= maxStress)
        {
            Debug.Log("Game Over! Granny is too stressed!");
        }
    }
}
