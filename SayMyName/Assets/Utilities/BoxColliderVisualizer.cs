using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class BoxColliderVisualizer : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Visualization Settings")]
    [Range(0f, 1f)] public float transparency = 0.3f;
    public Color color = new Color(0f, 1f, 0f, 0.3f); // default green

    private BoxCollider boxCollider;

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
            return;

        // Save the old color
        Color oldColor = Gizmos.color;

        // Set the semi-transparent color
        Color drawColor = color;
        drawColor.a = transparency;
        Gizmos.color = drawColor;

        // Build the matrix to correctly position and scale the cube
        Matrix4x4 cubeTransform = Matrix4x4.TRS(
            boxCollider.transform.TransformPoint(boxCollider.center),
            boxCollider.transform.rotation,
            boxCollider.transform.lossyScale
        );

        Gizmos.matrix = cubeTransform;

        // Draw the filled cube
        Gizmos.DrawCube(Vector3.zero, boxCollider.size);

        // Optional: draw a solid wireframe for clarity
        Gizmos.color = new Color(color.r, color.g, color.b, 1f);
        Gizmos.DrawWireCube(Vector3.zero, boxCollider.size);

        // Restore the original Gizmos color
        Gizmos.color = oldColor;


    }
#endif
}
