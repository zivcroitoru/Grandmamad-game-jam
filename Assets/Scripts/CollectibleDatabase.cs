using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CollectiblePrefabEntry
{
    public string itemName;
    public GameObject prefab;
}

public class CollectibleDatabase : MonoBehaviour
{
    public List<CollectiblePrefabEntry> collectibles;
    private Dictionary<string, GameObject> prefabDict;

void Awake()
{
    Debug.Log($"CollectibleDatabase on {gameObject.name} is initializing. Count = {collectibles.Count}");
    prefabDict = new Dictionary<string, GameObject>();
    foreach (var entry in collectibles)
    {
        Debug.Log($"Adding to prefabDict: \"{entry.itemName}\" → {(entry.prefab != null ? "OK" : "MISSING ❌")}");
        prefabDict[entry.itemName] = entry.prefab;
    }
}

public GameObject GetPrefab(string itemName)
{
    if (prefabDict == null || prefabDict.Count == 0)
    {
        Debug.LogError("prefabDict is empty!");
        return null;
    }

    if (prefabDict.ContainsKey(itemName))
    {
        Debug.Log($"Found prefab for \"{itemName}\" (length {itemName.Length})");
        return prefabDict[itemName];
    }

    Debug.LogWarning($"No prefab found for \"{itemName}\" (length {itemName.Length}). " +
                     $"Available keys:\n" + string.Join("\n", prefabDict.Keys.Select(k => $"\"{k}\" (len {k.Length})")));
    return null;
}


}
