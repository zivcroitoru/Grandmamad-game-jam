using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TriggerTest : MonoBehaviour
{
    public GameObject messagePanel;         // Panel holding the pickup text
    public TMP_Text messageText;            // Pickup text ("[E]\nItem")
    public TMP_Text collectedText;          // Text shown after collecting
    public float interactRange;             // Radius to detect items
    public KeyCode interactKey = KeyCode.E; // Interaction key
    public GameObject CLLCTUI;              // Collection UI popup (must have CanvasGroup)

    public static List<string> collectedItems = new List<string>();
    public static HashSet<string> usedItems = new HashSet<string>();

    private GameObject currentTarget;
    private Coroutine fadeCoroutine;
    private bool isShowingPrompt = false;
    private CanvasGroup cllctGroup;

    void Awake()
    {
        if (CLLCTUI != null)
            cllctGroup = CLLCTUI.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        currentTarget = null;
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);

        float closestDot = -1f;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Collectible")) continue;

            float dist = Vector3.Distance(hit.transform.position, transform.position);
            if (dist > interactRange) continue;

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
            messageText.text = $"<b>[E]</b> <b>{itemName}</b>";

            if (!isShowingPrompt)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeInMessage());
            }

            if (Input.GetKeyDown(interactKey))
            {
                Collect(currentTarget, itemName);
            }
        }
        else
        {
            if (isShowingPrompt)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeOutMessage());
            }
        }
    }

    void Collect(GameObject obj, string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log("Collected item: " + itemName);
        Debug.Log("Current total: " + collectedItems.Count);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pickupSFX); // 🔊 Play pickup sound

        StartCoroutine(ShowCollectedText(itemName));
        Destroy(obj);
    }


    IEnumerator ShowCollectedText(string itemName)
    {
        collectedText.text = $"+ {itemName}!";
        collectedText.gameObject.SetActive(true);

        if (cllctGroup != null)
        {
            cllctGroup.alpha = 0f;
            CLLCTUI.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(cllctGroup, 0f, 1f, 0.2f));
        }

        yield return new WaitForSeconds(1f);

        collectedText.gameObject.SetActive(false);

        if (cllctGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(cllctGroup, 1f, 0f, 0.3f));
            CLLCTUI.SetActive(false);
        }
    }

    IEnumerator FadeInMessage()
    {
        isShowingPrompt = true;
        messagePanel.SetActive(true);

        Color color = messageText.color;
        color.a = 0f;
        messageText.color = color;

        float duration = 0.3f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration);
            color.a = alpha;
            messageText.color = color;
            yield return null;
        }

        color.a = 1f;
        messageText.color = color;
    }

    IEnumerator FadeOutMessage()
    {
        isShowingPrompt = false;
        Color color = messageText.color;
        float duration = 0.3f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            color.a = alpha;
            messageText.color = color;
            yield return null;
        }

        messagePanel.SetActive(false);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / duration);
            group.alpha = alpha;
            yield return null;
        }

        group.alpha = to;
    }
}
