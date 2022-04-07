using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSystemControl : MonoBehaviour
{
    [Tooltip("Contains all objects player can store in the bag")]
    public List<GameObject> AllObjectsList = new List<GameObject>();
    [Tooltip("Contains current objects index in the bag")]
    public List<int> CurrentObjectIndexList = new List<int>();
    
    private bool bagIsOpening = false;
    private int currentInspectionObjectIndex = -1;

    public static BagSystemControl Instance { get; set; }
    // private int checkBagTimes = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && Time.timeScale != 0.0f)
        {
            if (PlayerControl.Instance.isInteractingWithObjects == true)
            {
                PlayerControl.Instance.ShowBagInfo("Cannot open bag while interacting", false, true);
            }
            else if (PlayerControl.Instance.IsShowingHint == false)
            {
                if (bagIsOpening == false)
                {
                    PlayerControl.Instance.checkBagTimes += 1;
                    // Debug.Log(PlayerControl.Instance.checkBagTimes);
                    openBag();
                }
                else
                {
                    closeBag();
                }
            }
        }

        if(bagIsOpening == true && CurrentObjectIndexList.Count > 0)
        {
            changeObj();
        }
    }

    public void ShowObject(int index)
    {
        InspectionSystem.Instance.TurnOn();
        AllObjectsList[index].SetActive(true);
    }

    public void HideObject(int index)
    {
        InspectionSystem.Instance.TurnOff();
        AllObjectsList[index].SetActive(false);
    }

    private void openBag()
    {
        InspectionSystem.Instance.TurnOn();

        bagIsOpening = true;
        if (CurrentObjectIndexList.Count > 0)
        {
            AllObjectsList[CurrentObjectIndexList[0]].SetActive(true);
            currentInspectionObjectIndex = 0;
        }
    }

    private void closeBag()
    {
        InspectionSystem.Instance.TurnOff();

        bagIsOpening = false;
        if (currentInspectionObjectIndex >= 0)
        {
            AllObjectsList[CurrentObjectIndexList[currentInspectionObjectIndex]].SetActive(false);
            currentInspectionObjectIndex = -1;
        }
    }

    private void changeObj()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentInspectionObjectIndex > 0)
            {
                AllObjectsList[CurrentObjectIndexList[currentInspectionObjectIndex]].SetActive(false);
                currentInspectionObjectIndex--;
                AllObjectsList[CurrentObjectIndexList[currentInspectionObjectIndex]].SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentInspectionObjectIndex < CurrentObjectIndexList.Count-1)
            {
                AllObjectsList[CurrentObjectIndexList[currentInspectionObjectIndex]].SetActive(false);
                currentInspectionObjectIndex++;
                AllObjectsList[CurrentObjectIndexList[currentInspectionObjectIndex]].SetActive(true);
            }

        }
    }

    public void AddObject(int index, bool dialoguePadding = false)
    {
        CurrentObjectIndexList.Add(index);
        PlayerControl.Instance.ShowBagInfo(AllObjectsList[index].name + " is added to bag", dialoguePadding, false);
    }

    public void RemoveObject(int index)
    {
        CurrentObjectIndexList.Remove(index);
        if (index == currentInspectionObjectIndex)
        {
            currentInspectionObjectIndex = 0;
        }
    }
}
