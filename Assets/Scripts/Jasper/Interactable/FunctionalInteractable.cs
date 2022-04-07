using UnityEngine;
using UnityEngine.Events;

public class FunctionalInteractable : Interactable
{
    [Header("Function")]
    public UnityEvent action;
    public bool singleUse = false;

    void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        action.Invoke();

        FinishInteracting();
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();

        if (singleUse == true)
        {
            enabled = false;
            playerControl.SetHandIcon(false);
        }
    }
}
