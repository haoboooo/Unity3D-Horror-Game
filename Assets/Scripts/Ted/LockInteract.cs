using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInteract : Interactable
{
    
    private LockController myLockController;
    // public Camera mainCamera;
    // public Camera lockCamera;

    private void Start()
    {
        base.Start();
        myLockController = GetComponent<LockController>();
        // myLockController.enabled = false;
        // lockCamera.enabled = false;
    }

    public override void Interact()
    {
        // mainCamera.enabled = false;
        // lockCamera.enabled = true;
        myLockController.enabled = true;
        base.Interact();
        // print("Interact with Lock");        
        
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();
        myLockController.enabled = false;
        // mainCamera.enabled = true;
        // lockCamera.enabled = false;
    }
}
