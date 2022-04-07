using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewKeyPadController : MonoBehaviour
{
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;
    public GameObject Button5;
    public GameObject Button6;
    public GameObject Button7;
    public GameObject Button8;
    public GameObject Button9;
    public GameObject Button0;
    string passwordText;
    int passwordLength;
    public int passwordLimit;
    public string correctPassword;
    public GameObject display;

    public LockedDoorIntearctable doorIntearctable;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("loaded");
        passwordText = "";
        passwordLength = 0;
        //passwordLimit = 4;
        //correctPassword = "1234";
        display.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((KeyCode)(48 + i)))
            {
                pressKey(i);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyUp((KeyCode)(48 + i)))
            {
                keyUp(i);
            }
        }
    }
    void pressKey(int number)
    {
        Debug.Log("按下了" + number + "键");
        if(passwordLength >= 4)
        {

        }
        else
        {
            passwordText = passwordText + (number).ToString();
            display.GetComponent<Text>().text = passwordText;
            passwordLength++;
            Vector3 vector = GameObject.Find("KEL_02_BUTTON_" + (number).ToString()).transform.localPosition;
            GameObject.Find("KEL_02_BUTTON_" + (number).ToString()).transform.localPosition = new Vector3(vector.x, vector.y, -0.005f);
            if (passwordLength == 4)
            {
                if (passwordText == correctPassword)
                {
                    Debug.Log("密码正确！");
                    display.GetComponent<Text>().color = Color.green;
                    StartCoroutine(UnlockDoor());
                }
                else
                {
                    Debug.Log("密码错误！");
                    display.GetComponent<Text>().color = Color.red;
                    StartCoroutine(wrongPassword());
                }
            }
        }
    }
    void keyUp(int number)
    {
        Vector3 vector = GameObject.Find("KEL_02_BUTTON_" + (number).ToString()).transform.localPosition;
        GameObject.Find("KEL_02_BUTTON_" + (number).ToString()).transform.localPosition = new Vector3(vector.x, vector.y, 0.005f);
    }
    IEnumerator wrongPassword()
    {
        Debug.Log(Time.time); // time before wait
        yield return new WaitForSeconds(0.75f);
        Debug.Log(Time.time); // time after wait
        Clear();
    }
    public void Clear()
    {
        passwordLength = 0;
        passwordText = "";
        display.GetComponent<Text>().text = passwordText;
        display.GetComponent<Text>().color = Color.white;
    }

    IEnumerator UnlockDoor()
    {
        yield return new WaitForSeconds(1);
        doorIntearctable.FinishInteracting();
    }
}
