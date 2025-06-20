using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StressManager : MonoBehaviour
{
    public float stress = 0f;
    public float maxStress = 100f;
    public float stressRate = 2f;

    public bool isInMamad = false;

    public Slider stressBar;
    public TMP_Text stressText;

    private Coroutine reduceCoroutine;

    void Update()
    {
        if (!isInMamad)
        {
            stress += Time.deltaTime * stressRate;
            stress = Mathf.Clamp(stress, 0f, maxStress);
        }

        UpdateUI();

        if (stress >= maxStress)
        {
            Debug.Log("Game Over! Granny is too stressed!");
        }
    }

    void UpdateUI()
    {
        if (stressBar != null)
            stressBar.value = stress / maxStress;

        if (stressText != null)
            stressText.text = $"{(stress / maxStress * 100f):0}%";
    }

    public void ReduceStressFromItems()
    {
        if (reduceCoroutine != null)
            StopCoroutine(reduceCoroutine);

        reduceCoroutine = StartCoroutine(AnimateStressReduction());
    }

    private IEnumerator AnimateStressReduction()
    {
        yield return new WaitForSeconds(1f); // wait before reducing

        int itemCount = TriggerTest.collectedItems.Count;
        float reduction = itemCount * 0.1f * maxStress;
        float targetStress = Mathf.Clamp(stress - reduction, 0f, maxStress);
        float duration = 1f;
        float elapsed = 0f;
        float startStress = stress;

        Debug.Log($"Starting animated stress reduction by {reduction}");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            stress = Mathf.Lerp(startStress, targetStress, elapsed / duration);
            UpdateUI();
            yield return null;
        }

        stress = targetStress;
        UpdateUI();

        Debug.Log($"Stress reduced to {stress}");
    }
}
