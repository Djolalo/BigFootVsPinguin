using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform currentTarget;
    public Transform defaultTarget;

    public float followSpeed = 5f;
    public float rotateSpeed = 5f;

    private bool isTransitioning = false;

    void LateUpdate()
    {
        if (currentTarget == null || isTransitioning) return;

        transform.position = Vector3.Lerp(
            transform.position,
            currentTarget.position,
            Time.deltaTime * followSpeed
        );

        Quaternion targetRot = Quaternion.LookRotation(
            currentTarget.forward,
            Vector3.up
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotateSpeed
        );
    }

    public IEnumerator CameraTransition(Transform target, float duration)
    {
        isTransitioning = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Slerp(
                startRot,
                Quaternion.LookRotation(target.forward),
                t
            );

            yield return null;
        }

        currentTarget = target;
        isTransitioning = false;
    }

    public IEnumerator JumpQTECamera(
        Transform qteTarget,
        float holdTime,
        float slowFactor
    )
    {
        // Slow motion
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Aller vers caméra QTE
        yield return CameraTransition(qteTarget, 0.2f);

        // Pause dramatique (temps réel)
        yield return new WaitForSecondsRealtime(holdTime);

        // Retour temps normal
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // 🔁 RETOUR caméra frontale
        yield return CameraTransition(defaultTarget, 0.4f);
    }
}
