using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BigBlit.Keypads;

public class NumPadLockControl : MonoBehaviour
{
    public List<GameObject> PadButtons;
    public LockedDoorIntearctable doorIntearctable;

    private bool pauseKeyDown = false;
    [HideInInspector]public int passwordLimit = 4;
    [HideInInspector] public string passwordText = "";
    public string correctPassword;

    private NumpadTextController textController;
    public int triedTimes = 0;

    void Start()
    {
        textController = GetComponent<NumpadTextController>();
        textController.UpdatePassword(passwordText);
    }

    void Update()
    {
        if (pauseKeyDown == false)
        {
            for (int i = 1; i < 10; i++)
            {
                if (Input.GetKeyDown((KeyCode)(48 + i)))
                {
                    triedTimes += 1;
                    pressKey(i);
                }
            }
            for (int i = 1; i < 10; i++)
            {
                if (Input.GetKeyUp((KeyCode)(48 + i)))
                {
                    releaseKey(i);
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && passwordText != "")
            {
                passwordText = passwordText.Substring(0, passwordText.Length - 1);
                textController.UpdatePassword(passwordText);
            }
        }
    }

    void pressKey(int number)
    {
        if(passwordText.Length < passwordLimit)
        {
            passwordText += (number).ToString();
            textController.UpdatePassword(passwordText);

            Vector3 buttonPos = PadButtons[number - 1].transform.localPosition;
            PadButtons[number - 1].transform.localPosition = new Vector3(buttonPos.x, buttonPos.y, -0.005f);

            if (passwordText.Length == 4)
            {
                releaseKey(passwordText[3] - 48);
                pauseKeyDown = true;
                if (passwordText == correctPassword)
                {
                    StartCoroutine(Pass());
                }
                else
                {
                    StartCoroutine(Fail());
                }
            }
        }
    }

    void releaseKey(int number)
    {
        Vector3 buttonPos = PadButtons[number - 1].transform.localPosition;
        PadButtons[number - 1].transform.localPosition = new Vector3(buttonPos.x, buttonPos.y, 0.005f);
    }

    public void Clear()
    {
        passwordText = "";
        pauseKeyDown = false;
        textController.UpdatePassword(passwordText);
    }

    IEnumerator Pass()
    {
        yield return new WaitForSeconds(0.75f);
        passwordText = "PASS";
        textController.UpdatePassword(passwordText);
        yield return new WaitForSeconds(1);
        doorIntearctable.FinishInteracting();
    }

    IEnumerator Fail()
    {
        yield return new WaitForSeconds(0.75f);
        passwordText = "FAIL";
        textController.UpdatePassword(passwordText);
        yield return new WaitForSeconds(1);
        Clear();
    }
}
