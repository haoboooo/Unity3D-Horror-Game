using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHeadControl : MonoBehaviour
{
    [Header("Neck")]
    public Transform neckTransform;
    public Vector3 neckStartRotation;
    public Vector3 neckTargetRotation;
    private Quaternion neckStartQuat;
    private Quaternion neckTargetQuat;

    [Header("Head")]
    public Transform headTransform;
    public Vector3 headStartRotation;
    public Vector3 headTargetRotation;
    private Quaternion headStartQuat;
    private Quaternion headTargetQuat;

    [Header("Timer Settings")]
    public float waitTime;
    public float animationTime;

    [Header("Hole Interact")]
    public HoleInteract holeInteract;

    private AudioSource audio;

    void Start()
    {
        neckStartQuat = Quaternion.Euler(neckStartRotation);
        neckTargetQuat = Quaternion.Euler(neckTargetRotation);
        headStartQuat = Quaternion.Euler(headStartRotation);
        headTargetQuat = Quaternion.Euler(headTargetRotation);

        audio = GetComponent<AudioSource>();
    }

    public void TurnHead()
    {
        StartCoroutine(TurnHeadCouroutine());
    }

    IEnumerator TurnHeadCouroutine()
    {
        holeInteract.canQuit = false;

        yield return new WaitForSeconds(waitTime);

        audio.Play();
        if (animationTime <= 0.0f)
        {
            neckTransform.localRotation = neckTargetQuat;
            headTransform.localRotation = headTargetQuat;
        }
        else
        {
            float startTimer = Time.realtimeSinceStartup;
            float timer = 0.0f;
            // Only for Webgl Build
            //while (true)
            //{
            //    neckTransform.localRotation = Quaternion.Slerp(neckStartQuat, neckTargetQuat, Mathf.Clamp01(timer / animationTime));
            //    headTransform.localRotation = Quaternion.Slerp(headStartQuat, headTargetQuat, Mathf.Clamp01(timer / animationTime));
            //    timer += Time.realtimeSinceStartup - startTimer;
            //    if (timer >= animationTime)
            //    {
            //        break;
            //    }
            //    yield return null;
            //}
            while (timer < animationTime)
            {
                neckTransform.localRotation = Quaternion.Slerp(neckStartQuat, neckTargetQuat, timer / animationTime);
                headTransform.localRotation = Quaternion.Slerp(headStartQuat, headTargetQuat, timer / animationTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(audio.clip.length / 2.0f);
        holeInteract.canQuit = true;
    }
}
