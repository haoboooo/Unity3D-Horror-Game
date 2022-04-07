using System.Collections;
using UnityEngine;

public class PianoInteractable : Interactable
{
    public Transform focusPointTransform;
    private PianoControl myPianoControl;

    private void Start()
    {
        base.Start();
        myPianoControl = GetComponent<PianoControl>();
    }

    public override void Interact()
    {
        base.Interact();
        PlayerControl.Instance.FocusOnObject(focusPointTransform, false);
        myPianoControl.enabled = true;
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();
        playerControl.StopFocusOnObject();
        myPianoControl.enabled = false;
        enabled = false;
        playerControl.SetHandIcon(false);
        BlinkControl.Instance.enabled = true;
        BlinkControl.Instance.CloseEye();
    }

    public override void QuitInteracting()
    {
        base.QuitInteracting();
        PlayerControl.Instance.StopFocusOnObject();
        myPianoControl.enabled = false;
        myPianoControl.Reset();
    }
}