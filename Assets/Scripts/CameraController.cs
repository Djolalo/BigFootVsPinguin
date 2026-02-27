using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform currentTarget;
    public Transform defaultTarget;

    public float followSpeed = 1f;
    public float rotateSpeed = 5f;

    private bool isTransitioning = false;
    private bool useSmoothFollow = false;

    void LateUpdate()
    {
        if (currentTarget == null || isTransitioning) return;

        if (useSmoothFollow)
        {
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
        else
        {
            transform.position = currentTarget.position;
            transform.rotation = Quaternion.LookRotation(currentTarget.forward, Vector3.up);
        }
    }

    public IEnumerator CameraTransition(Transform target, float duration)
    {
        isTransitioning = true;
        useSmoothFollow = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Slerp(startRot, Quaternion.LookRotation(target.forward), t);

            yield return null;
        }

        currentTarget = target;
        isTransitioning = false;
        useSmoothFollow = false;
    }

    public IEnumerator JumpQTECamera(
        Transform qteTarget,
        float holdTime,
        float slowFactor
    )
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // vers QTE
        yield return CameraTransition(qteTarget, 0.5f);

        // pause dramatique
        yield return new WaitForSecondsRealtime(holdTime);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // retour caméra frontale
        yield return CameraTransition(defaultTarget, 1.0f);
    }
}