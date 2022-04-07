using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIControl : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public GameObject Dialogue;
    public Image DialogueBox;
    public Text DialogueText;
    public float DialogueMessageLifeTime;
    private IEnumerator showDialogueCouroutine = null;
    private float DialogueTimer = 0.0f;
    private Color DialogueTextColor;
    private Color DialogueBoxColor;

    [Header("Bag Information Settings")]
    public GameObject BagInfo;
    public Image BagInfoBox;
    public Text BagInfoText;
    public float BagInfoMessageLifeTime;
    public float BagInfoShowTime = 1.0f;
    private IEnumerator showBagInfoCouroutine = null;
    private float BagInfoTimer = 0.0f;
    private Color BagInfoTextColor;
    private Color BagInfoBoxColor;

    private void Start()
    {
        DialogueTextColor = DialogueText.color;
        DialogueBoxColor = DialogueBox.color;
        BagInfoTextColor = BagInfoText.color;
        BagInfoBoxColor = BagInfoBox.color;
    }

    public void ShowDialogue(string mes)
    {
        if (showDialogueCouroutine != null)
        {
            StopCoroutine(showDialogueCouroutine);
        }
        showDialogueCouroutine = TurnOnDialogue(mes);
        StartCoroutine(showDialogueCouroutine);
    }

    IEnumerator TurnOnDialogue(string mes)
    {
        DialogueText.text = mes;
        DialogueTextColor.a = 1;
        DialogueBoxColor.a = 1;
        DialogueText.color = DialogueTextColor;
        DialogueBox.color = DialogueBoxColor;
        Dialogue.SetActive(true);
        DialogueTimer = 0.0f;

        while (DialogueTimer < DialogueMessageLifeTime)
        {
            DialogueTimer += Time.deltaTime;
            DialogueTextColor.a = CalculateAlpha(DialogueTimer, DialogueMessageLifeTime);
            DialogueText.color = DialogueTextColor;
            DialogueBoxColor.a = DialogueTextColor.a;
            DialogueBox.color = DialogueBoxColor;
            yield return null;
        }

        Dialogue.SetActive(false);
    }

    public void ShowBagInfo(string mes, bool immediate, float ExtraTime = 0.0f)
    {
        if (showBagInfoCouroutine != null)
        {
            StopCoroutine(showBagInfoCouroutine);
        }
        showBagInfoCouroutine = TurnOnBagInfo(mes, ExtraTime, immediate);
        StartCoroutine(showBagInfoCouroutine);
    }

    IEnumerator TurnOnBagInfo(string mes, float ExtraTime, bool immediate)
    {
        if (immediate == false)
        {
            yield return new WaitForSeconds(BagInfoShowTime + ExtraTime);
        }

        BagInfoText.text = mes;
        BagInfoTextColor.a = 1;
        BagInfoBoxColor.a = 1;
        BagInfoText.color = BagInfoTextColor;
        BagInfoBox.color = BagInfoBoxColor;
        BagInfo.SetActive(true);
        BagInfoTimer = 0.0f;

        while (BagInfoTimer < BagInfoMessageLifeTime)
        {
            BagInfoTimer += Time.deltaTime;
            BagInfoTextColor.a = CalculateAlpha(BagInfoTimer, BagInfoMessageLifeTime);
            BagInfoText.color = BagInfoTextColor;
            BagInfoBoxColor.a = BagInfoTextColor.a;
            BagInfoBox.color = BagInfoBoxColor;
            yield return null;
        }

        BagInfo.SetActive(false);
    }

    private float CalculateAlpha(float x, float n)
    {
        return 1.0f - Mathf.Pow(x / n, 4);
    }
}
