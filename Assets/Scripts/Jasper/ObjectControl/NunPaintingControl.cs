using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NunPaintingControl : MonoBehaviour
{
    public AnimationClip animClip;
    public float paddingTime;

    private AudioSource audio;
    private Animation anim;
    private Collider myCollider;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
        myCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            myCollider.enabled = false;
            anim.Play();
            StartCoroutine(PlayerSound());
        }
    }

    IEnumerator PlayerSound()
    {
        yield return new WaitForSeconds(animClip.length - paddingTime);
        audio.Play();
    }
}
