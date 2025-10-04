using UnityEngine;
using DG.Tweening;

public class RotateBackAndForthLocal : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Maximum tilt angle in degrees.")]
    public float rotationAmplitude = 10f;

    [Tooltip("Time for one full forward-backward rotation cycle (seconds).")]
    public float rotationDuration = 3f;

    [Tooltip("Rotation axis (e.g. X = forward/backward tilt, Z = side sway).")]
    public Vector3 rotationAxis = new Vector3(1f, 0f, 0f);

    private Quaternion startLocalRot;

    private Tween tween;

    void OnEnable()
    {
        // Store the starting local rotation
        startLocalRot = transform.localRotation;

        // Calculate the target rotation (local)
        Quaternion targetRot = startLocalRot * Quaternion.Euler(rotationAxis * rotationAmplitude);

        // Tween between start and target rotation in local space
        tween = transform.DOLocalRotateQuaternion(targetRot, rotationDuration / 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative(false);
    }

    private void OnDisable()
    {
        tween.Kill();
    }
}
