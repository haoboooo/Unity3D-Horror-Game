using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathRoomClosetInteract : Interactable
{
    [Header("BathRoom Closet Interact Settings")]
    public GameObject Lock;
    public GameObject Helmet;

    private AudioSource audioSource;
    private Animation myAnimation;

    void Start()
    {
        base.Start();
        solvedPreLock = false;
        audioSource = GetComponent<AudioSource>();
        myAnimation = GetComponent<Animation>();
    }

    public override void Interact()
    {
        base.Interact();
        InspectionSystem.Instance.TurnOn();
        Lock.SetActive(true);
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();
        InspectionSystem.Instance.TurnOff();

        solvedPreLock = true;
        playerControl.SetHintUI(false);
        playerControl.SetHandIcon(false);

        Lock.SetActive(false);
        audioSource.Play();
        myAnimation.Play();
        Helmet.SetActive(true);
        enabled = false;
    }

    public override void QuitInteracting()
    {
        base.QuitInteracting();
        InspectionSystem.Instance.TurnOff();
        Lock.SetActive(false);
    }
}
