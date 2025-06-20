using UnityEngine;
using System.Collections.Generic;

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
        prefabDict = new Dictionary<string, GameObject>();
        foreach (var entry in collectibles)
        {
            prefabDict[entry.itemName] = entry.prefab;
        }
    }

    public GameObject GetPrefab(string itemName)
    {
        return prefabDict.ContainsKey(itemName) ? prefabDict[itemName] : null;
    }
}
