using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxController : MonoBehaviour
{
    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void PlayAnimation()
    {
        anim.Play();
    }
}
