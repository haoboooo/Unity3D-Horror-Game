using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private AudioSource audio;

    private new Animation animation;
    private Interactable doorInteractable;
    private bool isOpen = false;
    private bool isRight = false;
    public float secondsElapsed = 0;

    void Start()
    {
        animation = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
        doorInteractable = GetComponentInChildren<Interactable>();
    }

    private void Update()
    {
        secondsElapsed += Time.deltaTime;
    }

    public void PlayerAnimation()
    {
        Vector3 fromPlayer = (PlayerControl.Instance.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(fromPlayer, transform.forward);

        if (isOpen == false)
        {
            if (dot >= 0)// player is at right side of the door, so should use left operation
            {
                animation.Play("LeftOpen");
                isRight = false;
            }
            else
            {
                animation.Play("RightOpen");
                isRight = true;
            }
            audio.clip = openDoorSound;
            audio.Play();
        }
        else
        {
            if (isRight)
            {
                animation.Play("RightClose");
            }
            else
            {
                animation.Play("LeftClose");
            }
            audio.clip = closeDoorSound;
            audio.Play();
        }
        isOpen = !isOpen;
    }

    public void Close()
    {
        isOpen = false;
    }

    public void StopAnimation()
    {
        doorInteractable.FinishInteracting();
    }
}
