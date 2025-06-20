using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TriggerTest : MonoBehaviour
{
    public TMP_Text messageText;           // "Press E to collect..."
    public TMP_Text collectedText;         // "Collected Bowl!"
    public float interactRange = 0.005f;
    public KeyCode interactKey = KeyCode.E;

    public static List<string> collectedItems = new List<string>();
    public static HashSet<string> usedItems = new HashSet<string>();

    private GameObject currentTarget;

 void Update()
{
    currentTarget = null;
    Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);

    float closestDot = -1f;

    foreach (var hit in hits)
    {
        if (!hit.CompareTag("Collectible"))
            continue;

        float dist = Vector3.Distance(hit.transform.position, transform.position);
        if (dist > interactRange)
            continue;

        Vector3 dirToHit = (hit.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToHit);

        if (dot > 0.01f && dot > closestDot)
        {
            currentTarget = hit.gameObject;
            closestDot = dot;
        }
    }

    if (currentTarget != null)
    {
        string itemName = currentTarget.name.Replace("(Clone)", "").Trim();
        messageText.text = $"Press E to collect {itemName}";
        messageText.gameObject.SetActive(true);

        if (Input.GetKeyDown(interactKey))
        {
            Collect(currentTarget, itemName);
        }
    }
    else
    {
        messageText.gameObject.SetActive(false);
    }
}

    void Collect(GameObject obj, string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log("Collected item: " + itemName);
        Debug.Log("Current total: " + collectedItems.Count);

        StartCoroutine(ShowCollectedText(itemName));
        Destroy(obj);
    }

    IEnumerator ShowCollectedText(string itemName)
    {
        collectedText.text = $"Collected {itemName}!";
        collectedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        collectedText.gameObject.SetActive(false);
    }
}
