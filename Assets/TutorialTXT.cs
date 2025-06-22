using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static bool IsTutorialActive { get; private set; }

    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;

    [Header("Mobile UI Elements")]
    public GameObject cameraJoystick;
    public GameObject moveJoystick;

    private bool tutorialDismissed = false;
    private bool isMobile;

    void Start()
    {
        IsTutorialActive = true;
        tutorialDismissed = false;
        isMobile = Application.isMobilePlatform;

        Time.timeScale = 0.01f;
        tutorialPanel.SetActive(true);
        AudioManager.Instance?.PauseMusic();

        // Display tutorial text based on platform
        tutorialText.text = isMobile
            ? "<size=30><b><color=#FFFFFF>Help Granny reach the shelter in time</color></b></size>\n" +
              "<size=24><color=#DDDDDD>Bring her comfort items along the way to keep her calm.</color></size>\n\n" +
              "<align=center><size=38><b><color=#FFD700>How long\ncan you last?</color></b></size></align>\n\n" +
              "<align=center><size=22><color=#EEEEEE>Move: <b>Left/Right Joystick</b></color></size></align>\n" +
              "<align=center><size=22><color=#EEEEEE>Collect: <b>Tap Button</b></color></size></align>\n\n" +
              "<align=center><size=20><color=#AAAAAA>Tap anywhere to begin</color></size></align>"
            : "<size=30><b><color=#FFFFFF>Help Granny reach the shelter in time</color></b></size>\n" +
              "<size=24><color=#DDDDDD>Bring her comfort items along the way to keep her calm.</color></size>\n\n" +
              "<align=center><size=38><b><color=#FFD700>How long\ncan you last?</color></b></size></align>\n\n" +
              "<align=center><size=22><color=#EEEEEE>Move: <b>WASD and mouse</b></color></size></align>\n" +
              "<align=center><size=22><color=#EEEEEE>Collect: <b>E key</b></color></size></align>\n\n" +
              "<align=center><size=20><color=#AAAAAA>Click to begin</color></size></align>";

        // Joysticks are not activated here — only previewed in UI
        if (isMobile)
        {
            if (cameraJoystick != null) cameraJoystick.SetActive(true);
            if (moveJoystick != null) moveJoystick.SetActive(true);
        }
    }

    void Update()
    {
        if (tutorialDismissed || !IsTutorialActive) return;

        if ((isMobile && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            (!isMobile && Input.GetMouseButtonDown(0)))
        {
            DismissTutorial();
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

        // Activate mobile controls after tutorial dismissed
        if (isMobile)
        {
            if (cameraJoystick != null) cameraJoystick.SetActive(true);
            if (moveJoystick != null) moveJoystick.SetActive(true);
        }
    }
}
