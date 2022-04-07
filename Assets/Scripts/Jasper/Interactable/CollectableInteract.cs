using UnityEngine;
using UnityEngine.Events;

public class CollectableInteract : Interactable
{
    [Header("CollectableInteraction Settings")]

    public UnityEvent action;

    public int BagIndex;
    [Tooltip("Interactable object that will be unlocked after you interact with this object")]
    public Interactable TargetInteractObject;

    public override void Interact()
    {
        base.Interact();
        BagSystemControl.Instance.ShowObject(BagIndex);
        if (TargetInteractObject != null)
        {
            TargetInteractObject.Unlock();
        }
    }

    public override void QuitInteracting()
    { 
        base.QuitInteracting();
        BagSystemControl.Instance.HideObject(BagIndex);
        BagSystemControl.Instance.AddObject(BagIndex, false);
        PlayerControl.Instance.SetHandIcon(false);
        gameObject.SetActive(false);
        action.Invoke();
    }
}
