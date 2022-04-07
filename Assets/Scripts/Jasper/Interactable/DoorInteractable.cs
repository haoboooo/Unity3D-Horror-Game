using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    private DoorControl doorControl;

    public override void Start()
    {
        base.Start();
        doorControl = GetComponentInParent<DoorControl>();
    }

    public override void Interact()
    {
        base.Interact();
        doorControl.PlayerAnimation();
    }
}