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

    [Header("Mobile UI Elements")]
    public GameObject cameraJoystick;
    public GameObject moveJoystick;
    public GameObject pickupButton;

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

tutorialText.text =
    "<size=30><b><color=#FFFFFF>Help Granny reach the</color></b></size>\n" +
    "<size=30><b><color=#FFFFFF>shelter in time</color></b></size>\n" +
    "<size=24><color=#DDDDDD>Bring her comfort items along the way</color></size>\n" +
    "<size=24><color=#DDDDDD>to keep her calm.</color></size>\n\n" +
    "<align=center><size=38><b><color=#FFD700>How long\ncan you last?</color></b></size></align>\n\n" +
    "<align=center><size=22><color=#EEEEEE>Move: <b>Left/Right Joystick</b></color></size></align>\n" +
    "<align=center><size=22><color=#EEEEEE>Collect: <b>Tap Button</b></color></size></align>\n\n" +
    "<align=center><size=20><color=#AAAAAA>Tap anywhere to begin</color></size></align>";

        // Show joysticks immediately if mobile
        if (isMobile)
        {
            if (cameraJoystick != null) cameraJoystick.SetActive(true);
            if (moveJoystick != null) moveJoystick.SetActive(true);
            if (pickupButton != null) pickupButton.SetActive(true);
        }
    }

    void Update()
    {
        if (tutorialDismissed || !IsTutorialActive)
            return;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                DismissTutorial();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
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
    }
}
