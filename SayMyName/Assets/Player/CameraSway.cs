using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public CharacterMovement player;   // your movement script (must have GetVelocity())
    public float maxTilt = 5f;         // max roll angle at top speed
    public float smooth = 8f;          // smoothing speed
    public float minSpeed = 1f;        // minimum speed before sway starts
    public float maxSpeed = 10f;       // speed at which sway reaches full maxTilt

    Quaternion defaultRot;

    void Start()
    {
        defaultRot = transform.localRotation;
    }

    void LateUpdate()
    {
        if (!player) return;

        Vector3 vel = player.GetVelocity();
        vel.y = 0f;
        float speed = vel.magnitude;

        // Determine sway intensity based on speed (0–1)
        float t = Mathf.InverseLerp(minSpeed, maxSpeed, speed);

        // Base rotation (return to default if not moving)
        Quaternion targetRot = defaultRot;

        if (t > 0f)
        {
            // Convert velocity to local camera space
            Vector3 localVel = transform.InverseTransformDirection(vel.normalized);

            // Roll proportional to strafe direction and speed factor
            float roll = -localVel.x * maxTilt * t;

            targetRot = Quaternion.Euler(defaultRot.eulerAngles.x, defaultRot.eulerAngles.y, roll);
        }

        // Smooth rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smooth);
    }
}
