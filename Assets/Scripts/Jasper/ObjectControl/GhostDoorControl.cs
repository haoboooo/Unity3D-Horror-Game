using UnityEngine;

public class GhostDoorControl : MonoBehaviour
{
    public AudioClip suddenDoorCloseSound;

    private Animation doorAnimation;
    private AudioSource doorAudioSource;
    private DoorControl doorControl;
    public SingleOpenDoorInteract doorInteract;

    void Start()
    {
        doorControl = GetComponentInParent<DoorControl>();
        doorAnimation = GetComponentInParent<Animation>();
        doorAudioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            doorAnimation.Play("SuddenLeftClose");
            doorAudioSource.clip = suddenDoorCloseSound;
            doorAudioSource.Play();
            gameObject.SetActive(false);
            doorInteract.enabled = true;
            doorInteract.Lock();
            doorControl.Close();
        }
    }
}
