using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenDoorInteractable : Interactable
{
    [Header("KitchenDoorInteractable Settings")]
    private Animation doorAnimation;
    private AudioSource audioSource;

    void Start()
    {
        base.Start();
        doorAnimation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        base.Interact();
        doorAnimation.Play();
        playerControl.SetHandIcon(false);
        FinishInteracting();
        audioSource.Play();
        enabled = false;
    }
}
