using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameplayUI;
    public TMP_Text gameOverText;
    private const string BestRoundsKey = "BestRoundsSurvived";

public void ShowGameOver(GameOverCause cause, int roundsSurvived)
    {
        gameplayUI.SetActive(false);
        gameOverScreen.SetActive(true);

        int bestRounds = PlayerPrefs.GetInt(BestRoundsKey, 0);
        bool isNewRecord = roundsSurvived > bestRounds;

        if (isNewRecord)
        {
            PlayerPrefs.SetInt(BestRoundsKey, roundsSurvived);
            PlayerPrefs.Save();
        }

        string reasonText = cause == GameOverCause.Stress
            ? "Granny got too stressed out!"
            : "YOUR TAKING TOO LONG";

        string roundText = $"<size=56><b>{roundsSurvived} Rounds</b></size>";
        string bestText = $"<size=24>Best: {Mathf.Max(roundsSurvived, bestRounds)} Rounds</size>";

        string styled =
            "<size=94><b>Game Over</b></size>\n\n" +
            $"<size=22>{reasonText}</size>\n\n" +
            $"{roundText}\n" +
            $"{bestText}\n";

        if (isNewRecord)
            styled += "<size=18><color=#FFD700><b>🏆 New Record!</b></color></size>\n\n";

        styled += Application.isMobilePlatform
            ? "<size=18><color=#FFFFFFAA>Tap to restart</color></size>"
            : "<size=18><color=#FFFFFFAA>R to Restart</color></size>";

        gameOverText.text = styled;
        gameOverText.ForceMeshUpdate();

        AudioManager.Instance?.PlayGameOverSFX();
        Time.timeScale = 0f;
    }
}
