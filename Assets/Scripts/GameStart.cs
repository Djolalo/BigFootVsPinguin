using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public CameraController cam;
    public Transform frontTarget;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        yield return cam.CameraTransition(frontTarget, 3f);
    }
}
