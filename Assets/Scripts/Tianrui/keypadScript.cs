using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keypadScript : MonoBehaviour
{
    public string passwordText;
    public int passwordLength;
    public int passwordLimit;
    public string correctPassword;
    public GameObject display;
    // Start is called before the first frame update
    void Start()
    {
        //passwordText = "";
        passwordText = "";
        passwordLength = 0;
        passwordLimit = 4;
        correctPassword = "1234";
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
        passwordText = passwordText + (number).ToString();
        display.GetComponent<Text>().text = passwordText;
        passwordLength++;
        Vector3 vector = GameObject.Find("key" + (number).ToString()).transform.localPosition;
        GameObject.Find("key" + (number).ToString()).transform.localPosition = new Vector3(vector.x, vector.y, -0.01f);
        if (passwordLength == 4)
        {
            if (passwordText == correctPassword)
            {
                Debug.Log("密码正确！");
                display.GetComponent<Text>().color = Color.green;
            }
            else
            {
                Debug.Log("密码错误！");
                display.GetComponent<Text>().color = Color.red;
                StartCoroutine(wrongPassword());
            }
        }
    }
    void keyUp(int number)
    {
        Vector3 vector = GameObject.Find("key" + (number).ToString()).transform.localPosition;
        GameObject.Find("key" + (number).ToString()).transform.localPosition = new Vector3(vector.x, vector.y, -0.5f);
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
}
