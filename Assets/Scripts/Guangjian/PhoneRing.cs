using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneRing : MonoBehaviour
{
    public AudioClip phoneRing;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

	
    void Update()
    {
	if(Input.GetKeyDown(KeyCode.E)) {
	    source.clip = phoneRing;
	    source.loop = true;
	    source.Play();
	}

	if(Input.GetKeyDown(KeyCode.R)) {
            source.Stop();
        }
    }
}
