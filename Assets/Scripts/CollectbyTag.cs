using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TriggerTest : MonoBehaviour
{
    public MobileInputManager mobileInput;
    public bool autoDetectMobile = true;
    private bool isMobile;

    public TMP_Text pickupButtonText;           // Text inside the pickup button
    public GameObject pickupButton;             // The button itself
    public TMP_Text collectedText;              // Text shown after collecting
    public GameObject CLLCTUI;                  // Popup UI with CanvasGroup
    public float interactRange;                 // Detection range
    public KeyCode interactKey = KeyCode.E;     // Key to pick up

    public static List<string> collectedItems = new List<string>();
    public static HashSet<string> usedItems = new HashSet<string>();

    private GameObject currentTarget;
    private CanvasGroup cllctGroup;

    void Awake()
    {
        if (CLLCTUI != null)
            cllctGroup = CLLCTUI.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (autoDetectMobile)
            isMobile = Application.isMobilePlatform;

        currentTarget = null;

        if (pickupButton != null)
            pickupButton.SetActive(false);

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

            if (pickupButton != null)
                pickupButton.SetActive(true);

            if (pickupButtonText != null)
                pickupButtonText.text = $"Pick up {itemName}";

            if ((!isMobile && Input.GetKeyDown(interactKey)) ||
                (isMobile && mobileInput != null && mobileInput.ConsumePickup()))
            {
                Collect(currentTarget, itemName);
            }
        }
    }

    void Collect(GameObject obj, string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log("Collected item: " + itemName);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pickupSFX);

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
