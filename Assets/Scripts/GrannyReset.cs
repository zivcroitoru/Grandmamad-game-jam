using UnityEngine;

public class GrannyResetPosition : MonoBehaviour
{
    public Transform[] spawnPoints; // Assign in Inspector
    private int resetIndex = -1;
    private int lastUsedIndex = -1;

    public void SetResetIndex(int index)
    {
        resetIndex = index;
    }

    public void ResetGranny()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        int chosenIndex = resetIndex;

        // Pick a random index if -1
        if (resetIndex == -1)
        {
            if (spawnPoints.Length == 1)
            {
                chosenIndex = 0;
            }
            else
            {
                // Try to avoid the same spawn point
                do
                {
                    chosenIndex = Random.Range(0, spawnPoints.Length);
                } while (chosenIndex == lastUsedIndex && spawnPoints.Length > 1);
            }
        }

        // Apply position and rotation
        transform.position = spawnPoints[chosenIndex].position;
        transform.rotation = spawnPoints[chosenIndex].rotation;

        Debug.Log($"Granny reset to spawn index {chosenIndex}");

        lastUsedIndex = chosenIndex;
        resetIndex = -1; // clear after use
    }
}
