using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;               // Directional Light de la scène
    public float dayDuration = 120f; // Durée d'une journée complète en secondes
    public float maxIntensity = 1f;  // Intensité du soleil en plein jour
    public float minIntensity = 0f;  // Intensité du soleil la nuit
    
    [Range(0f,360f)]
    public float noonAngle = 40;   // max rotation (optionnel)

    private float currentTime = 0f;  // Temps écoulé dans la journée

    void Update()
    {
        if (sun == null) return;

        currentTime += Time.deltaTime;

        float dayFraction = (currentTime / dayDuration) % 1f;

        float sunAngle = dayFraction * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, noonAngle, 0f); // 170 = direction du soleil dans ta scène
    }
}