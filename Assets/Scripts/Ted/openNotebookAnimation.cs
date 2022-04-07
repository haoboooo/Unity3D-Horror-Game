using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openNotebookAnimation : MonoBehaviour
{
    private Animation anim;
    public float secondsElapsed = 0;

    private void Start() {
        anim = GetComponent<Animation>();
    }

    private void Update() {
        secondsElapsed += Time.deltaTime;
    }

    public void OpenBook()
    {
        StartCoroutine("openBook");
    }

    IEnumerator openBook()
    {   
        anim.Play();
        yield return null;
    }
}
