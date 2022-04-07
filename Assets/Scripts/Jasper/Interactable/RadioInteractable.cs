using UnityEngine;

public class RadioInteractable : Interactable
{
    [Header("RadioInteractable Settings")]
    public GameObject radio;
    private AudioListener playerAduioListener;

    void Start()
    {
        base.Start();
        playerAduioListener = playerControl.playerCamera.GetComponent<AudioListener>();
    }

    public override void Interact()
    {
        base.Interact();
        InspectionSystem.Instance.TurnOn();
        radio.SetActive(true);

        playerAduioListener.enabled = false;
    }

    public override void QuitInteracting()
    {
        base.QuitInteracting();
        InspectionSystem.Instance.TurnOff();
        radio.SetActive(false);
        playerAduioListener.enabled = true;
    }
}
