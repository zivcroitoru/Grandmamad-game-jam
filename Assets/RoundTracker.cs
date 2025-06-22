using UnityEngine;
using TMPro;

public class RoundTracker : MonoBehaviour
{
    public int RoundsPassed { get; private set; } = 0;

    [Header("UI (Optional)")]
    public TMP_Text roundsText;

    public void IncrementRound()
    {
        RoundsPassed++;

        UpdateUI();
    }

    public void ResetRounds()
    {
        RoundsPassed = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (roundsText != null)
        {
            roundsText.text = $"Rounds: {RoundsPassed}";
        }
    }
}
