using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class MapInteractable : Interactable
{
    [Header("MapInteractable Settings")]

    public Transform focusPointTransform;

    private MapControl mapControl;
    AnalyticsResult ar;
    private float startTime = 0;
    private float solveTime;

    void Start()
    {
        base.Start();
        solvedPreLock = false;
        mapControl = GetComponentInChildren<MapControl>();
    }

    public override void Interact()
    {
        base.Interact();
        if (startTime == 0) 
        {
            startTime = mapControl.secondsElapsed;
        } 
        PlayerControl.Instance.FocusOnObject(focusPointTransform, true);
        mapControl.enabled = true;
    }

    public override void FinishInteracting()
    {
        base.FinishInteracting();
        solvedPreLock = true;
        playerControl.SetHintUI(false);

        // report custom event
        solveTime = mapControl.secondsElapsed - startTime;
        PlayerControl.Instance.solvePuzzles += 1;
        ReportSolve3LMap(solveTime, mapControl.triedTimes);
        // debug
        ar = Analytics.CustomEvent("solve_3L_Map");
        Debug.Log("solve_3L_Map_Result = " + ar.ToString() + "Use_time = " + solveTime.ToString() + "tried_time=" + mapControl.triedTimes);
        PlayerControl.Instance.StopFocusOnObject();
        mapControl.enabled = false;
    }

    public override void QuitInteracting()
    {
        base.QuitInteracting();
        PlayerControl.Instance.StopFocusOnObject();
        mapControl.enabled = false;
    }
    public void ReportSolve3LMap(float sTime, int triedTime){
        // custom event, report the time used to solve the lock
        AnalyticsEvent.Custom("solve_3L_Map", new Dictionary<string, object>
        {
            { "solve_time", sTime },
            { "tried_time", triedTime}
        });
    }
}