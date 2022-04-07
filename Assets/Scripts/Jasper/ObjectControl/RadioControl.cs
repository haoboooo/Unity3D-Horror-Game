using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControl : MonoBehaviour
{
    [Header("Button Settings")]
    public Transform turningButton;
    public float turningSpeed;
    private float currentRotation = 0.0f;
    private Vector3 turningButtonRot;

    [Header("Indicator Settings")]
    public Transform indicator;
    public float startPosition;
    public float endPosition;
    private Vector3 indicatorPosition;

    [Header("AudioSettings")]
    public AudioClip paddingClip;
    public List<AudioClip> audioClipList;
    private AudioSource audioSource;
    private int currentIndex = 0;
    private float paddingLength;
    private int unitRotation;
    private IEnumerator switchCoroutine = null;
    private bool alreadySwitched = false;
    private IEnumerator audioCoroutine = null;

    private void Awake()
    {
        if (audioClipList == null || audioClipList.Count <= 0)
        {
            Debug.LogError("Empty Clip List");
        }
        
        turningButtonRot = turningButton.localRotation.eulerAngles;
        indicatorPosition = indicator.localPosition;

        audioSource = GetComponent<AudioSource>();
        paddingLength = paddingClip.length;
        unitRotation = 360 / audioClipList.Count;

        audioSource.clip = audioClipList[0];
    }

    private void OnEnable()
    {
        audioCoroutine = PlayMusic();
        StartCoroutine(audioCoroutine);
    }

    private void OnDisable()
    {
        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
        }
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
        }
    }

    private void Update()
    {
        float action = Input.GetAxis("Horizontal");
        if (Mathf.Abs(action) < 0.001f)
        {
            return;
        }

        float targetRotation = currentRotation + action * turningSpeed * Time.deltaTime;
        if (targetRotation >= 360.0f || targetRotation < 0.0f)
        {
            return;
        }

        float currentRemainder = currentRotation % unitRotation;
        float targetRemainder = targetRotation % unitRotation;
        if (Mathf.Abs(currentRemainder - targetRemainder) > 20.0f && alreadySwitched == false)
        {
            currentIndex += currentRemainder > targetRemainder ? 1 : -1;
            switchCoroutine = SwitchMusic();
            StartCoroutine(switchCoroutine);
        }

        currentRotation = targetRotation;
        turningButtonRot.z = currentRotation;
        turningButton.localRotation = Quaternion.Euler(turningButtonRot);

        indicatorPosition.x = Mathf.Lerp(startPosition, endPosition, currentRotation / 360.0f);
        indicator.localPosition = indicatorPosition;
    }

    IEnumerator SwitchMusic()
    {
        audioSource.clip = paddingClip;
        audioSource.Play();
        alreadySwitched = true;

        yield return new WaitForSeconds(paddingLength);

        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
        }
        audioCoroutine = PlayMusic();
        StartCoroutine(audioCoroutine);
        alreadySwitched = false;
    }

    IEnumerator PlayMusic()
    {
        audioSource.clip = audioClipList[currentIndex];
        float musicTime = audioClipList[currentIndex].length;
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(musicTime + 1.0f);
        }
    }
}