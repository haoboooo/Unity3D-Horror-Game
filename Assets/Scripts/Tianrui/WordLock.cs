using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLock : MonoBehaviour
{
    public LockedDoorIntearctable doorInteract;

    public GameObject Wheel1;
    public GameObject Wheel2;
    public GameObject Wheel3;
    public GameObject Button;
    int CurrentChosenWheel = 0;
    public string PasswordWord1;
    public string PasswordWord2;
    public string PasswordWord3;
    string Wheel1Word = "";
    string Wheel2Word = "";
    string Wheel3Word = "";
    int Wheel1Num = 0;
    int Wheel2Num = 0;
    int Wheel3Num = 0;
    public GameObject Lock;
    string[] PasswordDic = { "A", "B", "C", "D", "E", "F" };
    float RotateDegree = 0;
    public int triedTimes = 0;

    private Animation anim;
    private bool solved = false;

    private AudioSource audio;

    private void Start()
    {
        anim = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (solved == false)
        {
            OutlineCurrentWheel();
            if (Input.GetKeyDown(KeyCode.A))
            {
                triedTimes += 1;
                CurrentChosenWheel--;
                if (CurrentChosenWheel < 0) CurrentChosenWheel = 2;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                triedTimes += 1;
                CurrentChosenWheel++;
                if (CurrentChosenWheel > 2) CurrentChosenWheel = 0;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                switch (CurrentChosenWheel)
                {
                    case 0:
                        LockRotate(Wheel1, true);
                        Wheel1Num--;
                        if (Wheel1Num < 0) Wheel1Num = 5;
                        break;
                    case 1:
                        LockRotate(Wheel2, true);
                        Wheel2Num--;
                        if (Wheel2Num < 0) Wheel2Num = 5;
                        break;
                    case 2:
                        LockRotate(Wheel3, true);
                        Wheel3Num--;
                        if (Wheel3Num < 0) Wheel3Num = 5;
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                switch (CurrentChosenWheel)
                {
                    case 0:
                        LockRotate(Wheel1, false);
                        Wheel1Num++;
                        if (Wheel1Num > 5) Wheel1Num = 0;
                        break;
                    case 1:
                        LockRotate(Wheel2, false);
                        Wheel2Num++;
                        if (Wheel2Num > 5) Wheel2Num = 0;
                        break;
                    case 2:
                        LockRotate(Wheel3, false);
                        Wheel3Num++;
                        if (Wheel3Num > 5) Wheel3Num = 0;
                        break;
                }
            }
            if (ValidatePassword() == true)
            {
                Unlock();
            }
        }
    }

    void LockRotate(GameObject Wheel, bool Direction)
    {
        if (Direction == false)
        {
            Wheel.transform.Rotate(-60, 0, 0);
        }
        else
        {
            Wheel.transform.Rotate(60, 0, 0);
        }
    }

    bool ValidatePassword()
    {
        if (PasswordDic[Wheel1Num] == PasswordWord1 && PasswordDic[Wheel2Num] == PasswordWord2 && PasswordDic[Wheel3Num] == PasswordWord3)
        {
            return true;
        }
        return false;
    }

    void Unlock()
    {
        StartCoroutine("TranslateAndRotateMetal");
    }

    IEnumerator TranslateAndRotateMetal()
    {
        audio.Play();
        anim.Play();
        solved = true;
        doorInteract.canQuit = false;

        yield return new WaitForSeconds(anim.GetClip("unlock").length + 0.5f);

        doorInteract.canQuit = true;
        doorInteract.FinishInteracting();
    }

    void OutlineCurrentWheel()
    {
        switch (CurrentChosenWheel)
        {
            case 0:
                TurnOffOutline();
                Wheel1.GetComponent<cakeslice.Outline>().OnEnable();
                break;
            case 1:
                TurnOffOutline();
                Wheel2.GetComponent<cakeslice.Outline>().OnEnable();
                break;
            case 2:
                TurnOffOutline();
                Wheel3.GetComponent<cakeslice.Outline>().OnEnable();
                break;
        }
    }

    void TurnOffOutline()
    {
        Wheel1.GetComponent<cakeslice.Outline>().OnDisable();
        Wheel2.GetComponent<cakeslice.Outline>().OnDisable();
        Wheel3.GetComponent<cakeslice.Outline>().OnDisable();
    }
}
