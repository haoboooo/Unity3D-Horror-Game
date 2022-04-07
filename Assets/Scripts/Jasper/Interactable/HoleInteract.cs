using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleInteract : Interactable
{
    public ZombieHeadControl zombieControl;
    public Transform focusPointTransform;

    public override void Interact()
    {
        base.Interact();
        PlayerControl.Instance.FocusOnObject(focusPointTransform, false);
        zombieControl.TurnHead();
    }

    public override void QuitInteracting()
    {
        enabled = false;
        PlayerControl.Instance.StopFocusOnObject();
        PlayerControl.Instance.SetHandIcon(false);
        Vector3 pos = zombieControl.transform.position;
        pos.x = 6.78f;
        zombieControl.transform.position = pos;
    }
}
