using UnityEngine;

public class GrannyResetPosition : MonoBehaviour
{
    public Transform[] resetPoints; // Assign in Inspector
    public int currentIndex = 0;    // Index of the current stage/level/etc.

    public void ResetGranny()
    {
        if (resetPoints != null && resetPoints.Length > currentIndex)
        {
            transform.position = resetPoints[currentIndex].position;
            transform.rotation = resetPoints[currentIndex].rotation;
        }
        else
        {
            Debug.LogWarning("Invalid Granny reset point index or list is empty.");
        }
    }

    public void SetResetIndex(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, resetPoints.Length - 1);
    }
}
