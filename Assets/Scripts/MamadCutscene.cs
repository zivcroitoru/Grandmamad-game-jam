using UnityEngine;
using System.Collections;
using System;

public class MamadCutscene : MonoBehaviour
{
    public GameObject granny;
    public Transform mamadSitPosition;
    public GameObject timerUI;
    public Camera mainCamera;
    public Camera mamadCamera;
    public ScreenFader fader;
    public StressManager stressManager;
    public CollectibleSpawner spawner;
    public RoundTimer roundTimer;

    public void PlayCutscene(Action onComplete)
    {
        StartCoroutine(CutsceneRoutine(onComplete));
    }

    IEnumerator CutsceneRoutine(Action onComplete)
    {
        roundTimer.PauseTimer(); // ⏸️ Pause the timer

        yield return StartCoroutine(fader.FadeOut(0.2f));
        AudioManager.Instance.StopMusic();

        AudioManager.Instance.PlayMamadMusic(); // 🔊 Mamad background music

        // Move Granny and disable control
        granny.transform.position = mamadSitPosition.position;
        granny.transform.rotation = mamadSitPosition.rotation;

        timerUI.SetActive(false);

        var disabler = granny.GetComponent<GrannyMovementDisabler>();
        if (disabler != null) disabler.SetEnabled(false);

        // Switch cameras
        mainCamera.gameObject.SetActive(false);
        mamadCamera.gameObject.SetActive(true);

        // Play sit animation
        var animator = granny.GetComponent<Animator>();
        if (animator != null)
            animator.Play("Sitting");

        // Stress logic and item display
        stressManager.isInMamad = true;
        stressManager.ReduceStressFromItems();
        spawner.SpawnCollectedItems();

        yield return StartCoroutine(fader.FadeIn(0.5f));
        yield return new WaitForSeconds(7f);
        yield return StartCoroutine(fader.FadeOut(0.5f));

        yield return StartCoroutine(ResetScene()); // now with delay

        yield return StartCoroutine(fader.FadeIn(0.5f));

        roundTimer.ResumeTimer(); // ▶️ Resume the timer

        onComplete?.Invoke();
    }

    IEnumerator ResetScene()
    {
        yield return new WaitForSecondsRealtime(0.1f); // short delay for stability

        timerUI.SetActive(true);
        roundTimer.ResetTimer(60f);

        var disabler = granny.GetComponent<GrannyMovementDisabler>();
        if (disabler != null) disabler.SetEnabled(true);

        var resetScript = granny.GetComponent<GrannyResetPosition>();
        if (resetScript != null)
        {
            resetScript.SetResetIndex(-1); // Use random reset point
            resetScript.ResetGranny();
        }

        var animator = granny.GetComponent<Animator>();
        if (animator != null)
            animator.Play("Idle");

        mainCamera.gameObject.SetActive(true);
        mamadCamera.gameObject.SetActive(false);
        stressManager.isInMamad = false;
        AudioManager.Instance.PlayRoundMusic();
    }
}
