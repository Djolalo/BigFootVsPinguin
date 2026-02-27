using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Sun Settings")]
    public Light sun;
    public float dayDuration = 120f;

    [Range(0f,360f)]
    public float noonAngle = 40;
    private float currentTime = 0f;

    void Update()
    {
        if (sun == null) return;

        currentTime += Time.deltaTime;
        float dayFraction = (currentTime / dayDuration) % 1f;

        float sunAngle = (dayFraction * 360f) % 360f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, noonAngle, 0f);
    }
}