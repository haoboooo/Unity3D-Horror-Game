using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneControl : MonoBehaviour
{
    public GameObject TwistedPainting;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PhoneRing()
    {
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(3);
        source.Play();
    }

    public void PickUpPhone()
    {
        TwistedPainting.SetActive(true);
        source.Stop();
    }
}
