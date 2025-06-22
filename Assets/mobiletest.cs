using UnityEngine;
using TMPro;

public class MobileTMPTextActivator : MonoBehaviour
{
    private TMP_Text label;

    void Awake()
    {
        label = GetComponent<TMP_Text>();

        if (Application.isMobilePlatform)
        {
            label.text = "✅ MOBILE PLATFORM ACTIVE";
            label.gameObject.SetActive(true);
            Debug.Log("[MobileTMPTextActivator] Running on mobile");
        }
        else
        {
            label.gameObject.SetActive(false);
            Debug.Log("[MobileTMPTextActivator] Not mobile — hiding label");
        }
    }
}
