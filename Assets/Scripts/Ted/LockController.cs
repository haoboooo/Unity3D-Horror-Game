using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{   
    // 控制密码锁的开关，outline轮廓
    // outline使用：在Assets > Resource > Ted > OutlineEffect > OutlneEffect中，
    // 将OutlineEffect拖至Main Camera下，Outline拖至想要展示轮廓的物体下就可以使用。
    // 在脚本中控制Outline显示可以使用GameObject.GetComponent<cakeslice.Outline>().OnEnable()/OnDisable();

    int CurrentChosenWheel = 0;
    // 三个密码盘的游戏对象
    public GameObject Wheel1;
    public GameObject Wheel2;
    public GameObject Wheel3;
    public GameObject MetalPiece;
    // 三个密码
    public int PasswordDigit1;
    public int PasswordDigit2;
    public int PasswordDigit3;
    int Wheel1Num = 0;
    int Wheel2Num = 0;
    int Wheel3Num = 0;
    float RotateDegree = 0;
    float TranslateDistance = 0;
    private LockInteract lockInteract;

    public Interactable myInteract;
    private AudioSource audio;

    private bool solved = false;
    private Animation anim;
    public float secondsElapsed = 0;
    public int triedTimes = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        lockInteract = GetComponent<LockInteract>();
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
        // 开始时关闭Outline
        TurnOffOutline();
    }
    // Update is called once per frame
    void Update()
    {
        secondsElapsed += Time.deltaTime;
        if (solved == false)
        {
            // 将当前选中的密码盘显示Outline
            OutlineCurrentWheel();
            // WS键控制上下选择密码盘
            if (Input.GetKeyDown(KeyCode.W))
            {
                CurrentChosenWheel--;
                if (CurrentChosenWheel < 0) CurrentChosenWheel = 2;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CurrentChosenWheel++;
                if (CurrentChosenWheel > 2) CurrentChosenWheel = 0;
            }
            // AD控制左右   
            if (Input.GetKeyDown(KeyCode.A))
            {
                triedTimes += 1;
                switch (CurrentChosenWheel)
                {
                    case 0:
                        // Wheel1.GetComponent<cakeslice.Outline>().OnEnable();
                        // 旋转密码盘
                        LockRotate(Wheel1, true);
                        Wheel1Num--;
                        if (Wheel1Num < 0) Wheel1Num = 9;
                        break;
                    case 1:
                        // Wheel2.GetComponent<cakeslice.Outline>().OnEnable();
                        LockRotate(Wheel2, true);
                        Wheel2Num--;
                        if (Wheel2Num < 0) Wheel2Num = 9;
                        break;
                    case 2:
                        // Wheel3.GetComponent<cakeslice.Outline>().OnEnable();
                        LockRotate(Wheel3, true);
                        Wheel3Num--;
                        if (Wheel3Num < 0) Wheel3Num = 9;
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                triedTimes += 1;
                switch (CurrentChosenWheel)
                {
                    case 0:
                        LockRotate(Wheel1, false);
                        Wheel1Num++;
                        if (Wheel1Num > 9) Wheel1Num = 0;
                        break;
                    case 1:
                        LockRotate(Wheel2, false);
                        Wheel2Num++;
                        if (Wheel2Num > 9) Wheel2Num = 0;
                        break;
                    case 2:
                        LockRotate(Wheel3, false);
                        Wheel3Num++;
                        if (Wheel3Num > 9) Wheel3Num = 0;
                        break;
                }
            }
            // 如果当前密码有效，开锁
            if (ValidatePassword() == true)
            {
                Unlock();
            }
        }
    }
    // 将密码盘每次旋转36度
    void LockRotate(GameObject Wheel, bool Direction)
    {
        if (Direction == false) 
        {   
            Wheel.transform.Rotate(0, 36, 0);                     
        }
        else
        {            
            Wheel.transform.Rotate(0, -36, 0);            
        }
    }
    // 验证密码
    bool ValidatePassword()
    {
        if (Wheel1Num == PasswordDigit1 && Wheel2Num == PasswordDigit2 && Wheel3Num == PasswordDigit3) 
        {
            return true;
        }
        return false;
    }

    void Unlock()
    {
        solved = true;
        TurnOffOutline();
        StartCoroutine("TranslateAndRotateMetal");
    }

    // 控制开锁动画，将metalpiece上移并旋转
    IEnumerator TranslateAndRotateMetal()
    {
        audio.Play();
        anim.Play();
        myInteract.canQuit = false;

        yield return new WaitForSeconds(anim.GetClip("unlock").length + 0.5f);

        myInteract.canQuit = true;
        myInteract.FinishInteracting();
    }
    // 控制显示当前选中密码盘的outline
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
    // 关闭所有outline
    void TurnOffOutline() 
    {
        Wheel1.GetComponent<cakeslice.Outline>().OnDisable();
        Wheel2.GetComponent<cakeslice.Outline>().OnDisable();
        Wheel3.GetComponent<cakeslice.Outline>().OnDisable();
    }
}
