using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    public Light fireLight;
    public float minIntensity = .9f;
    public float maxIntensity = 1.0f;
    public float flickerSpeed = 10f;

    private float timer = 0f;

    void Update()
    {
        if (fireLight == null) return;

        timer += Time.deltaTime;

        if (timer >= 1f / flickerSpeed)
        {
            fireLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = 0f;
        }
    }
}