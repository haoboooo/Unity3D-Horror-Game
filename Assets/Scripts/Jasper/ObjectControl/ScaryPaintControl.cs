using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryPaintControl : MonoBehaviour
{
    public List<GameObject> disappearObjects;
    public AudioClip scaryClip;

    void Start()
    {
        StartCoroutine(StartScary());
    }

    IEnumerator StartScary()
    {
        Vector3 playerForward;
        Vector3 paintForward = transform.up;
        paintForward.y = 0.0f;
        float dot;
        while (true)
        {
            playerForward = PlayerControl.Instance.forward;
            playerForward.y = 0.0f;
            dot = Vector3.Dot(playerForward, paintForward);
            if (dot >= 0.5f)
            {
                PlayerControl.Instance.playerAudio.clip = scaryClip;
                PlayerControl.Instance.playerAudio.Play();
                foreach (GameObject obj in disappearObjects)
                {
                    obj.SetActive(false);
                }
                break;
            }

            yield return null;
        }

        while (true)
        {
            playerForward = PlayerControl.Instance.forward;
            playerForward.y = 0.0f;
            dot = Vector3.Dot(playerForward, paintForward);
            if (dot <= -0.5f)
            {
                gameObject.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}
