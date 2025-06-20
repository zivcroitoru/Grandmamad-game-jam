using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Needed for accessing List

public class StressManager : MonoBehaviour
{
    public float stress = 0f;
    public float maxStress = 100f;
    public float stressRate = 2f;

    public bool isInMamad = false;

    public Slider stressBar;
    public TMP_Text stressText;

    void Update()
    {
        if (!isInMamad)
        {
            stress += Time.deltaTime * stressRate;
            stress = Mathf.Clamp(stress, 0f, maxStress);
        }

        if (stressBar != null)
            stressBar.value = stress / maxStress;

        if (stressText != null)
            stressText.text = $"{(stress / maxStress * 100f):0}%";

        if (stress >= maxStress)
        {
            Debug.Log("Game Over! Granny is too stressed!");
        }
    }

    public void ReduceStressFromItems()
    {
        int itemCount = TriggerTest.collectedItems.Count;
        float reduction = itemCount * 0.1f * maxStress;

        stress -= reduction;
        stress = Mathf.Clamp(stress, 0f, maxStress);

        Debug.Log($"Reduced stress by {reduction} from {itemCount} items");

    }
}
