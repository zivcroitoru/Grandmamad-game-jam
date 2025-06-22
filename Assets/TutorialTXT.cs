using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static bool IsTutorialActive { get; private set; }

    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    public Button dismissButton;

    private bool tutorialDismissed = false;

    void Start()
    {
        IsTutorialActive = true;
        tutorialDismissed = false;

        Time.timeScale = 0.01f;
        tutorialPanel.SetActive(true);
        AudioManager.Instance?.PauseMusic();

        if (dismissButton != null)
            dismissButton.onClick.AddListener(DismissTutorial);

        bool isMobile = Application.isMobilePlatform;

        tutorialText.text = isMobile
            ? "<size=30><b><color=#FFFFFF>Help Granny reach the shelter in time</color></b></size>\n" +
              "<size=24><color=#DDDDDD>Bring her comfort items along the way to keep her calm.</color></size>\n\n" +
              "<size=38><b><color=#FFD700>How long can you last?</color></b></size>\n\n" +
              "<size=22><color=#EEEEEE>Move: <b>Left/Right Joystick</b>\nCollect: <b>Tap Button</b></color></size>\n\n" +
              "<size=20><color=#AAAAAA>Tap anywhere to begin</color></size>"
            : "<size=30><b><color=#FFFFFF>Help Granny reach the shelter in time</color></b></size>\n" +
              "<size=24><color=#DDDDDD>Bring her comfort items along the way to keep her calm.</color></size>\n\n" +
              "<size=38><b><color=#FFD700>How long can you last?</color></b></size>\n\n" +
              "<size=22><color=#EEEEEE>Move: <b>WASD</b> and <b>Mouse</b>\nCollect: <b>E</b></color></size>\n\n" +
              "<size=20><color=#AAAAAA>Click anywhere to begin</color></size>";
    }

    void Update()
    {
        if (tutorialDismissed || !IsTutorialActive)
            return;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                DismissTutorial();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                DismissTutorial();
            }
        }
    }

    public void DismissTutorial()
    {
        if (tutorialDismissed) return;

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        tutorialDismissed = true;
        IsTutorialActive = false;

        AudioManager.Instance?.ResumeMusic();

        PlayerPrefs.SetInt("TutorialShown", 1);
        PlayerPrefs.Save();
    }
}
