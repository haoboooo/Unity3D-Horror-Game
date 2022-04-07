using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class BookInteract : Interactable
{
    [Header("Diary Interact Settings")]

    public GameObject ColorLock;
    public int InspectDiaryBookIndex;

    public Collider closeBookCollider;
    public Collider openBookCollider;

    private openNotebookAnimation diaryControl;
    AnalyticsResult ar;
    private float startTime = 0;
    private float solveTime;

    private AudioSource audioSource;

    private void Start()
    {
        base.Start();
        solvedPreLock = false;
        myCollider = closeBookCollider;
        diaryControl = GetComponent<openNotebookAnimation>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        base.Interact();
        if (startTime == 0) 
        {
            startTime = diaryControl.secondsElapsed;
        }   
        Debug.Log("LCStartTime = " + startTime.ToString());

        if (solvedPreLock == false)
        {
            InspectionSystem.Instance.TurnOn();
            ColorLock.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            BagSystemControl.Instance.AddObject(InspectDiaryBookIndex);
            PlayerControl.Instance.SetHandIcon(false);
        }
    }

    public override void FinishInteracting()
    {
        //finish diarylock
        base.FinishInteracting();
        // add custom params in analytical events: seconds played
        solveTime = diaryControl.secondsElapsed - startTime;
        
        if (solvedPreLock == false)
        {
            InspectionSystem.Instance.TurnOff();
            ColorLock.SetActive(false);
            solvedPreLock = true;
            playerControl.SetHintUI(false);

            // report custom event
            PlayerControl.Instance.solvePuzzles += 1;
            ReportSolve3LDiaryLock(solveTime, ColorLock.GetComponent<LockController>().triedTimes);
            ar = Analytics.CustomEvent("solve_diary_lock");
            Debug.Log("solve_3L_diarylock_Result = " + ar.ToString() + "tried_time=" + ColorLock.GetComponent<LockController>().triedTimes);

            diaryControl.OpenBook();
            audioSource.Play();
            myCollider = openBookCollider;
        }
    }

    public override void QuitInteracting()
    {
        //not finish diary lock, press space to quit
        base.QuitInteracting();
        if (solvedPreLock == false)
        {
            InspectionSystem.Instance.TurnOff();
            ColorLock.SetActive(false);
        }
    }
    public void ReportSolve3LDiaryLock(float sTime, int triedTimes){
        // custom event, report the time used to solve the lock
        AnalyticsEvent.Custom("solve_diary_lock", new Dictionary<string, object>
        {
            { "solve_time", sTime },
            { "tried_time", triedTimes}
        });
    }
}
