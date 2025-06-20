using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TriggerTest : MonoBehaviour
{
    public TMP_Text messageText;
    public static List<string> collectedItems = new List<string>();

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Collectible"))
    {
        string itemName = other.name.Replace("(Clone)", "").Trim();
        collectedItems.Add(itemName);

        Debug.Log("Collected item: " + itemName);
        Debug.Log("Current total: " + collectedItems.Count);

        Destroy(other.gameObject);
    }
}

}
