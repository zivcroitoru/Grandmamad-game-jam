using UnityEngine;
using UnityEditor;

public static class RemoveChildCollidersTool
{
    [MenuItem("Tools/Remove Child Colliders from Selected Object")]
    public static void RemoveChildColliders()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        Collider[] colliders = selected.GetComponentsInChildren<Collider>(true);
        int count = 0;

        foreach (Collider col in colliders)
        {
            if (col.gameObject != selected)
            {
                Undo.DestroyObjectImmediate(col);
                count++;
            }
        }

        Debug.Log($"✅ Removed {count} child colliders from \"{selected.name}\"");
    }
}
