using UnityEngine;

public class FireLightControl : MonoBehaviour
{
    public float fireFlickerMultipler = 10.0f;
    private Light myLight;
    private float lightIntensity;
    private float lightRange;

    private void Start()
    {
        myLight = GetComponent<Light>();
        lightIntensity = myLight.intensity;
        lightRange = myLight.range;
    }

    private void Update()
    {
        myLight.intensity = Mathf.Lerp(lightIntensity - 0.1f, lightIntensity + 0.1f, Mathf.Cos(Time.time * fireFlickerMultipler));
        myLight.range = Mathf.Lerp(lightRange - 0.1f, lightRange + 0.1f, Mathf.Cos(Time.time * fireFlickerMultipler));
    }
}
