using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleOpenDoorInteract : DoorInteractable
{
    private bool alreadyTriggeredGhost;

    public override void Start()
    {
        base.Start();
        alreadyTriggeredGhost = false;
    }

    public override void Interact()
    {
        base.Interact();
        if (alreadyTriggeredGhost == false)
        {
            alreadyTriggeredGhost = true;
            PlayerControl.Instance.SetHandIcon(false);
            enabled = false;
        }
    }
}
