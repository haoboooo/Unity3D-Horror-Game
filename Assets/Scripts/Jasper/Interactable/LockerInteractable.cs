using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerInteractable : Interactable
{ 
    private Animation lockerControl;
    private AudioSource audioSource;

    [Header("Locker Settings")]
    public GameObject loveLetter;
    public GameObject lockerLight;
    public AnimationClip lockerAnimation;

    void Start()
    {
        base.Start();
        lockerControl = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        base.Interact();
        lockerControl.Play();
        audioSource.Play();
        loveLetter.SetActive(true);
        lockerLight.SetActive(true);
        enabled = false;
        playerControl.SetHandIcon(false);
        BagSystemControl.Instance.RemoveObject(4);
        StartCoroutine(EnableLoveLetter());
        FinishInteracting();
    }

    IEnumerator EnableLoveLetter()
    {
        yield return new WaitForSeconds(lockerAnimation.length);
        loveLetter.GetComponent<CollectableInteract>().enabled = true;
    }
}
