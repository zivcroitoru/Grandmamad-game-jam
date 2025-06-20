using UnityEngine;
using System.Collections.Generic;

public class MamadManager : MonoBehaviour
{
    public Transform mamadDecorationArea; // Place where items appear
    public GameObject[] itemPrefabs; // Array of prefabs matching collectible names
    public float stressReductionPerItem = 5f;

    public StressManager stressManager; // Reference to your StressManager

    void Start()
    {
        DecorateMamad();
        ReduceStress();
    }

    void DecorateMamad()
    {
        foreach (string itemName in TriggerTest.collectedItems)
        {
            GameObject prefab = FindPrefabByName(itemName);
            if (prefab != null)
            {
                Instantiate(prefab, GetRandomPosition(), Quaternion.identity, mamadDecorationArea);
            }
            else
            {
                Debug.LogWarning("No prefab found for: " + itemName);
            }
        }
    }

    void ReduceStress()
    {
        float totalReduction = TriggerTest.collectedItems.Count * stressReductionPerItem;
        stressManager.stress -= totalReduction;
        stressManager.stress = Mathf.Clamp(stressManager.stress, 0, stressManager.maxStress);
        Debug.Log($"Stress reduced by {totalReduction}");
    }

    GameObject FindPrefabByName(string name)
    {
        foreach (GameObject obj in itemPrefabs)
        {
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    Vector3 GetRandomPosition()
    {
        Vector3 area = new Vector3(2, 0, 2); // spread range
        return mamadDecorationArea.position + new Vector3(
            Random.Range(-area.x, area.x),
            0,
            Random.Range(-area.z, area.z)
        );
    }
}
