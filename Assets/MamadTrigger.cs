using UnityEngine;
using System.Collections;

public class MamadTrigger : MonoBehaviour
{
    public Transform[] spawnPoints; // Assign in Inspector
    public Transform mamadDisplayZone; // Assign in Inspector
    public CollectibleDatabase database; // Assign in Inspector

    public GameObject granny; // Reference to player/granny GameObject
    public Animator grannyAnimator; // Animator with sitting animation
    public Transform mamadSitPosition; // Target position to place Granny in Mamad

    public Camera mainCamera;
    public Camera mamadCamera;
    public ScreenFader fader; // Assign ScreenFader script
    public StressManager stressManager;

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
        // Fade to black
        yield return StartCoroutine(fader.FadeOut(0.2f));

        // Teleport granny to Mamad sit position
        granny.transform.position = mamadSitPosition.position;
        granny.transform.rotation = mamadSitPosition.rotation;

        // Switch to mamad camera
        mainCamera.gameObject.SetActive(false);
        mamadCamera.gameObject.SetActive(true);

        // Play sit animation
        grannyAnimator.Play("Sitting"); // Use the name of your sitting animation

        // Reduce stress
        stressManager.isInMamad = true;
        stressManager.ReduceStressFromItems();

        // Spawn collected items
        Debug.Log("Starting to spawn collected items... Count: " + TriggerTest.collectedItems.Count);

        for (int i = 0; i < TriggerTest.collectedItems.Count && i < spawnPoints.Length; i++)
        {
            string itemName = TriggerTest.collectedItems[i];
            Debug.Log($"Trying to spawn: \"{itemName}\"");

            GameObject prefab = database.GetPrefab(itemName);
            if (prefab != null)
            {
                Transform spawnPoint = spawnPoints[i];
                GameObject instance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                instance.transform.SetParent(mamadDisplayZone, true); // Keep world scale
                Debug.Log($"Spawned \"{itemName}\" at {spawnPoint.position}");
            }
            else
            {
                Debug.LogWarning($"No prefab found for \"{itemName}\" in the CollectibleDatabase!");
            }
        }

        // Fade back in
        yield return StartCoroutine(fader.FadeIn(0.5f));
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stressManager.isInMamad = false;
        }
    }
}
