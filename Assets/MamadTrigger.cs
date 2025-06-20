using UnityEngine;
using System.Collections;

public class MamadTrigger : MonoBehaviour
{
    public GameObject granny;
    public Transform mamadSitPosition;
    public Transform nextStartPoint;

    public TimerManager timerScript;
    public GameObject timerUI;

    public Camera mainCamera;
    public Camera mamadCamera;
    public ScreenFader fader;
    public StressManager stressManager;

    public CollectibleSpawner spawner; // NEW

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(EnterMamadCutscene());
        }
    }

    IEnumerator EnterMamadCutscene()
    {
        yield return StartCoroutine(fader.FadeOut(0.2f));

        // Move Granny
        granny.transform.position = mamadSitPosition.position;
        granny.transform.rotation = mamadSitPosition.rotation;

        // Disable timer and movement
        timerUI.SetActive(false);
        granny.GetComponent<MonoBehaviour>().enabled = false;

        // Camera switch
        mainCamera.gameObject.SetActive(false);
        mamadCamera.gameObject.SetActive(true);

        // Sit animation
        granny.GetComponent<Animator>().Play("Sitting");

        // Stress and spawn
        stressManager.isInMamad = true;
        stressManager.ReduceStressFromItems();
        spawner.SpawnCollectedItems();

        yield return StartCoroutine(fader.FadeIn(0.5f));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(fader.FadeOut(0.3f));

        ResetAfterMamad();

        yield return StartCoroutine(fader.FadeIn(0.5f));
    }

    public void ResetAfterMamad()
    {
        timerUI.SetActive(true);
        timerScript.ResetTimer(60f);

        granny.GetComponent<MonoBehaviour>().enabled = true;

        var resetScript = granny.GetComponent<GrannyResetPosition>();
        if (resetScript != null)
            resetScript.ResetGranny();

        granny.GetComponent<Animator>().Play("Idle");

        mainCamera.gameObject.SetActive(true);
        mamadCamera.gameObject.SetActive(false);

        stressManager.isInMamad = false;
    }
}
