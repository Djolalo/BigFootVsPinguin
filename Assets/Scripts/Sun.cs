using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Sun Settings")]
    public Light sun;                 // Directional Light de la scène
    public float dayDuration = 120f;  // Durée d'une journée complète en secondes
    public float maxSunIntensity = 1f;
    public float minSunIntensity = 0f;
    [Range(0f, 360f)]
    public float noonAngle = 40f;     // Angle Y du soleil
    public float sunAngle = 0f;     // Test

    [Header("Ambient Settings")]
    public Color dayAmbientColor = Color.white;
    public Color nightAmbientColor = Color.black;

    private float currentTime = 0f;

    void Update()
    {
        if (sun == null) return;

        // Avancer le temps
        currentTime += Time.deltaTime;
        float dayFraction = (currentTime / dayDuration) % 1f;

        // Rotation du soleil (0 → 360°)
        sunAngle = (dayFraction * 360f) % 360f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, noonAngle, 0f);

        // Calcul de l'intensité du soleil
        // Nuit : 0 → 180°, Jour : 180 → 360°
        float intensity;
        if (sunAngle <= 30f)
        {
            float t = sunAngle / 30f;
            intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, t); // Levant
        }
        else if (sunAngle <= 150f)
        {
            intensity = maxSunIntensity; // Jour
        }
        else if (sunAngle <= 180f)
        {
            float t = (180f - sunAngle) / 30f;
            intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, t); // Couchant
        }
        else
        {
            intensity = minSunIntensity; // Nuit
        }
        sun.intensity = intensity;

        // Ajuster la couleur de la lumière ambiante
        RenderSettings.ambientLight = Color.Lerp(nightAmbientColor, dayAmbientColor, intensity);
        RenderSettings.ambientIntensity = intensity;
    }
}
