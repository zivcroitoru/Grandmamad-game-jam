using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject gameplayUI;

    public void BeginGame()
    {
        gameplayUI.SetActive(true);
        AudioManager.Instance?.PlayRoundMusic();
    }
}
