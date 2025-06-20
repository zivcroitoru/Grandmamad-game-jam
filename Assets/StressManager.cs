using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public float stress = 0f;
    public float maxStress = 100f;
    public float stressRate = 2f;

    public float firstAlarmTime = 30f;
    public float secondAlarmTime = 60f;

    public Slider stressBar; // Link to UI bar

    void Update()
    {
        if (Time.time > secondAlarmTime)
            stress += Time.deltaTime * (stressRate * 3f);
        else if (Time.time > firstAlarmTime)
            stress += Time.deltaTime * (stressRate * 2f);
        else
            stress += Time.deltaTime * stressRate;

        stress = Mathf.Clamp(stress, 0f, maxStress);

        if (stressBar != null)
            stressBar.value = stress / maxStress;

        if (stress >= maxStress)
        {
            Debug.Log("Game Over! Granny is too stressed!");
            // Load GameOver screen or freeze input
        }
    }
}
