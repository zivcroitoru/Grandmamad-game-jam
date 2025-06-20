using UnityEngine;

public class GrannyResetPosition : MonoBehaviour
{
    public Transform[] resetPoints; // Assign in Inspector
    public int currentIndex = 0;

    public void ResetGranny()
    {
        if (resetPoints != null && resetPoints.Length > 0)
        {
            // Use currentIndex unless it's -1 (which means "random")
            int indexToUse = currentIndex;

            if (currentIndex == -1)
                indexToUse = Random.Range(0, resetPoints.Length);

            transform.position = resetPoints[indexToUse].position;
            transform.rotation = resetPoints[indexToUse].rotation;

            Debug.Log($"Granny reset to point {indexToUse}");
        }
        else
        {
            Debug.LogWarning("No reset points assigned.");
        }
    }

    public void SetResetIndex(int index)
    {
        currentIndex = index; // Use -1 to trigger random selection
    }
}
