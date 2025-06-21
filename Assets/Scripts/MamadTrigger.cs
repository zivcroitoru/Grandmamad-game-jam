using UnityEngine;

public class MamadTrigger : MonoBehaviour
{
    public MamadCutscene cutscene;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            cutscene.PlayCutscene(() => hasTriggered = false);
        }
    }
}
