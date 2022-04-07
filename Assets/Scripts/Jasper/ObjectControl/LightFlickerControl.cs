using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerControl : MonoBehaviour
{
    private Light myLight;
    private float originalLightIntensity;
    private bool isFlickering;
    private IEnumerator flickerCoroutine;

    void Start()
    {
        myLight = GetComponent<Light>();
        originalLightIntensity = myLight.intensity;
        isFlickering = false;
        flickerCoroutine = null;
    }

    void Update()
    {
        if (isFlickering == false)
        {
            flickerCoroutine = StartFlicker();
            StartCoroutine(flickerCoroutine);
        }
    }

    IEnumerator StartFlicker()
    {
        isFlickering = true;
        myLight.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        myLight.enabled = false;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        myLight.enabled = true;
        isFlickering = false;
    }

    private void OnDisable()
    {
        isFlickering = true;
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }
        myLight.enabled = true;
        myLight.intensity = originalLightIntensity;
    }

    private void OnEnable()
    {
        isFlickering = false;
    }
}
