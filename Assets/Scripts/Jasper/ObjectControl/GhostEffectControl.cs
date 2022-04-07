using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffectControl : MonoBehaviour
{
    [Header("Light Settings")]
    public Light flickLight;
    public Light normalLight;
    public GameObject candleEffect;
    public int flickTimes;
    public List<float> flicerPeriods;
    public float darkTimeLength;
    private float flickLightOriginalIntensity;
    private bool candleIsLight;

    [Header("Ghost Settings")]
    public Transform ghostObject;
    public float scareTimeLength;
    public GameObject jumpScare;

    [Header("Other Settings")]
    public SingleOpenDoorInteract bedroomDoor;
    public GameObject BlockPlayer;

    private AudioSource audioSource;
    private bool scaryEffectStart = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        flickLightOriginalIntensity = flickLight.intensity;
        candleIsLight = candleEffect.activeInHierarchy;
        candleEffect.SetActive(false);
    }

    void Update()
    {
        if (scaryEffectStart == true)
        {
            return;
        }

        float dot = Vector3.Dot(PlayerControl.Instance.forward, ghostObject.forward);
        if (dot < -0.65f)
        {
            StartCoroutine(StartScaryEffect());
        }
    }

    IEnumerator StartScaryEffect()
    {
        scaryEffectStart = true;

        int flickCount = 0;
        while (flickCount < flickTimes)
        {
            foreach (float time in flicerPeriods)
            {
                flickLight.enabled = !flickLight.enabled;
                yield return new WaitForSeconds(time);
            }
            flickCount++;
        }

        flickLight.enabled = false;
        normalLight.enabled = false;

        yield return new WaitForSeconds(darkTimeLength);

        ghostObject.gameObject.SetActive(false);
        jumpScare.SetActive(true);
        audioSource.Play();

        yield return new WaitForSeconds(scareTimeLength);

        jumpScare.SetActive(false);
        BlockPlayer.SetActive(false);
        flickLight.intensity = flickLightOriginalIntensity;
        flickLight.enabled = true;
        normalLight.enabled = true;
        candleEffect.SetActive(candleIsLight);
        bedroomDoor.Unlock();
    }
}
