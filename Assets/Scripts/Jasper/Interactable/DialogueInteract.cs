using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteract : Interactable
{
    public override void Interact()
    {
        base.Interact();
        StartCoroutine(FinishDialogue());
    }

    IEnumerator FinishDialogue()
    {
        yield return new WaitForSeconds(playerControl.DialogueTime());
        FinishInteracting();
    }
}
