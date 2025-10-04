using UnityEngine;
using DG.Tweening;

public class Levitate : MonoBehaviour
{
    [Header("Levitation Settings")]
    [Tooltip("How far the object moves up and down from its starting local position.")]
    public float moveAmplitude = 0.5f;

    [Tooltip("Time for one full up-down cycle (seconds).")]
    public float moveDuration = 2f;

    private Vector3 startLocalPos;

    private Tween levitateTween;

    void OnEnable()
    {
        // Save the starting local position
        startLocalPos = transform.localPosition;

        // Move up by moveAmplitude, then back down in a Yoyo loop
        levitateTween = transform.DOLocalMoveY(startLocalPos.y + moveAmplitude, moveDuration / 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative(false); // ensures motion is relative to startLocalPos
    }

    void OnDisable()
    {
        levitateTween?.Kill();
    }
}
