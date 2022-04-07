using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInteractable : Interactable
{
    [Header("ScaleInteractable Settings")]
    public Transform focusPointTransform;
    public ScaleControl scaleControl;
    public Animation fridgeAnimation;
    public GameObject fireWood;

    void Start()
    {
        base.Start();
        solvedPreLock = false;
    }

    public override void Interact()
    {
        base.Interact();
        PlayerControl.Instance.FocusOnObject(focusPointTransform, false);
        scaleControl.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();
        PlayerControl.Instance.StopFocusOnObject();
        scaleControl.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enabled = false;
        playerControl.SetHandIcon(false);

        fridgeAnimation.Play();
        fireWood.SetActive(true);
    }

    public override void QuitInteracting()
    {
        base.QuitInteracting();
        PlayerControl.Instance.StopFocusOnObject();
        scaleControl.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
