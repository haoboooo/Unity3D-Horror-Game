using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetInteractable : Interactable
{
    [Header("HelmetInteractable Settings")]
    public GameObject myCamera;
    private HelmetControl helmetControl;


    void Start()
    {
        base.Start();
        helmetControl = GetComponentInParent<HelmetControl>();
    }

    public override void Interact()
    {
        base.Interact();

        playerControl.playerMovement.enabled = false;
        playerControl.playerCamera.SetActive(false);
        playerControl.SetCrossHair(false);

        myCamera.SetActive(true);
        helmetControl.enabled = true;
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();

        playerControl.SetHandIcon(false);
        enabled = false;

        playerControl.playerMovement.enabled = true;
        playerControl.playerCamera.SetActive(true);
        playerControl.SetCrossHair(true);

        myCamera.SetActive(false);
        helmetControl.enabled = false;
    }

    public override void QuitInteracting()
    {
        base.QuitInteracting();

        playerControl.playerMovement.enabled = true;
        playerControl.playerCamera.SetActive(true);
        playerControl.SetCrossHair(true);

        myCamera.SetActive(false);
        helmetControl.enabled = false;
    }
}
