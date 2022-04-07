using System.Collections;
using UnityEngine;

public class FirePlaceInteractable : Interactable
{
    [Header("FireplaceInteractable Settings")]
    public GameObject piano;
    public GameObject pianoLight;
    public GameObject campFire;
    public Animation firePlaceSetAnimation;
    private AudioSource audioSource;
    private bool ignited = false;
   
    void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        base.Interact();

        if (ignited == false)
        {
            campFire.SetActive(true);
            BagSystemControl.Instance.RemoveObject(8);
            ignited = true;
            FinishInteracting();
            playerControl.SetHandIcon(false);
            playerControl.SetLockIcon(true);
            Lock();
            message = "Maybe I can light it up...";
        }
        else
        {
            StartCoroutine(StartMove());
        }
    }

    IEnumerator StartMove()
    {
        firePlaceSetAnimation.Play();
        audioSource.Play();
        piano.SetActive(true);
        playerControl.SetHandIcon(false);
        enabled = false;

        yield return new WaitForSeconds(2.5f);

        pianoLight.SetActive(true);
    }
}
