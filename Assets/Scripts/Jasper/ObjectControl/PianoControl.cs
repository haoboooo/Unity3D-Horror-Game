using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PianoControl : MonoBehaviour
{
    public List<int> LyricSequence;
    public List<KeyCode> KeycodeSequence;

    private int CurrentIndex;
    private PianoInteractable pianoInteract;

    void Start()
    {
        CurrentIndex = 0;
        pianoInteract = GetComponent<PianoInteractable>();
    }

    void Update()
    {
        for (int i = 0; i < KeycodeSequence.Count; i++)
        {
            if (Input.GetKeyDown(KeycodeSequence[i]) == true)
            {
                if (LyricSequence[CurrentIndex] == i + 1)
                {
                    CurrentIndex++;
                }
                else
                {
                    CurrentIndex = 0;
                }
            }
        }
        if (CurrentIndex == LyricSequence.Count)
        {
            CurrentIndex = 0;
            StartCoroutine(FinishLyric());
        }
    }

    IEnumerator FinishLyric()
    {
        yield return new WaitForSeconds(1);
        pianoInteract.FinishInteracting();
    }

    public void Reset()
    {
        CurrentIndex = 0;
    }
}