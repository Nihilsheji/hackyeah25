using UnityEngine;
using DG.Tweening;

public class MoveDownWithDOTween : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Distance to move down (positive value).")]
    public float moveDistance = 2f;
    public float delay = 1f;

    [Tooltip("Duration of the downward movement in seconds.")]
    public float moveDuration = 0.5f;

    [Tooltip("Easing type for the movement animation.")]
    public Ease moveEase = Ease.OutQuad;

    /// <summary>
    /// Moves the GameObject down by the specified moveDistance.
    /// </summary>
    public void MoveDown()
    {
        Vector3 targetPosition = transform.position - new Vector3(0, moveDistance, 0);
        transform.DOMove(targetPosition, moveDuration)
                 .SetEase(moveEase)
                 .SetDelay(delay);
    }
}
