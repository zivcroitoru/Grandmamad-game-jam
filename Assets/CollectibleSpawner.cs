using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform parentZone;
    public CollectibleDatabase database;

  public void SpawnCollectedItems()
{
    Debug.Log("Spawning collected items...");

    for (int i = 0; i < TriggerTest.collectedItems.Count && i < spawnPoints.Length; i++)
    {
        string itemName = TriggerTest.collectedItems[i];
        GameObject prefab = database.GetPrefab(itemName);

        Debug.Log($"[{i}] Retrieved prefab for \"{itemName}\" → {(prefab != null ? "Not Null ✅" : "NULL ❌")}");

        if (prefab != null)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject instance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            if (instance == null)
            {
                Debug.LogError($"[{i}] Instantiate FAILED for \"{itemName}\" ❌");
            }
            else
            {
                instance.transform.SetParent(parentZone, true);
                Debug.Log($"[{i}] Spawned \"{itemName}\" at {spawnPoint.position}");
            }
        }
        else
        {
            Debug.LogWarning($"[{i}] No prefab found for \"{itemName}\"");
        }
    }
}

}
