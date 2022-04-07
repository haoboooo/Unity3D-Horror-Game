using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    public AudioClip toneClip;
    public int toneIndex;
    public float pressedRotation = 8.0f;

    private AudioSource pianoAudioSource;
    private PianoControl pianoLyricControl;
    private KeyCode myKeyCode;

    void Start()
    {
        pianoAudioSource = GetComponentInParent<AudioSource>();
        pianoLyricControl = GetComponentInParent<PianoControl>();
        myKeyCode = pianoLyricControl.KeycodeSequence[toneIndex];
    }

    void Update()
    {
        if(pianoLyricControl.enabled == true)
        {
            if (Input.GetKeyDown(myKeyCode))
            {
                pianoAudioSource.PlayOneShot(toneClip);
                Vector3 rot = transform.localRotation.eulerAngles;
                rot.x = pressedRotation;
                transform.localRotation = Quaternion.Euler(rot);
            }
            else if (Input.GetKey(myKeyCode) == false)
            {
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
}
