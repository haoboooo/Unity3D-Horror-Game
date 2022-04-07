// // using this package before the code
// using UnityEngine;
// using UnityEngine.Analytics;
// //**************************************************************************************************
// // IMPORTANT: FOR BETTER VIEW EFFECT, CANCEL ALL THE COMMENTS USING CTRL + /
// // 使用前请注释掉所有内容
// //**************************************************************************************************
// public class AnalyticTemplate : MonoBehaviour
// {
//     //use for debug, test if analytic event works
//     AnalyticsResult ar;
//     float secondsElapsed = 0;
//     // get the used time, add to Control.cs
//     private float startTime = 0;
//     private float solveTime;
//     private void Update()
//     {
//         // count the time used, need to be in Controller.cs file or the item would become unInteractable
//         secondsElapsed += Time.deltaTime;
//     }
//     // some function you need to set up an event
//     private void someFunction(){
//         // set up a levelstart event
//         AnalyticsEvent.LevelStart("3L_diary_lock");
//         ar = AnalyticsEvent.LevelStart("3L_diary_lock");
//         if (startTime == 0) 
//         {
//             startTime = diaryControl.secondsElapsed;
//         }  
//         Debug.Log("LCStart = " + ar.ToString() + startTime.ToString());
//         //AnalyticsEvent.LevelStart("some word.....");
//         // for more events, see 
//         //https://docs.unity3d.com/cn/current/Manual/UnityAnalyticsStandardEvents.html

//         // set up a custom event
//         // To set up a custom event with params, first initialize a dict and add the params need
//         // in this example, I use the diaryControl.secondsElapsed as param, so you need to init it in diaryControl.cs,
//         // and add this param when you need to send a event(or the time would be wrong)
//         Dictionary<string, object> customParams = new Dictionary<string, object>();
//         customParams.Add("seconds_played", diaryControl.secondsElapsed);

//         // set up a level complete event, same as level start   
//         solveTime = diaryControl.secondsElapsed - startTime;
//         Dictionary<string, object> customParams = new Dictionary<string, object>();
//         customParams.Add("seconds_played", solveTime.ToString());     

//             // Finish event
//             AnalyticsEvent.LevelComplete("3L_diary_lock", customParams);
//             ar = AnalyticsEvent.LevelComplete("3L_diary_lock");
//             Debug.Log("LCFinish = " + ar.ToString() + diaryControl.secondsElapsed.ToString() + "SolveTime=" + solveTime.ToString());
//             // report custom event
//             ReportSolve3LDiaryLock(solveTime);

//             // Quit event
//             // AnalyticsEvent.LevelQuit("diary_lock", customParams);
//             // ar = AnalyticsEvent.LevelQuit("diary_lock");
//             // Debug.Log("LQResult = " + ar.ToString() + diaryControl.secondsElapsed.ToString());


//         // Debug for standard events
//         // ar = AnalyticsEvent.LevelComplete("diary_lock");
//         // Debug.Log("LCFinish = " + ar.ToString() + diaryControl.secondsElapsed.ToString());
//         // Debug for custom events
//         ReportRotateGlobe();
//         ar = Analytics.CustomEvent("rotate_globe");
//         Debug.Log("rotate_globe_Result = " + ar.ToString());
//     }   

//     // set up a custom event like below func
//     public void ReportRotateGlobe(){
//         AnalyticsEvent.Custom("rotate_globe", new Dictionary<string, object>
//         {
//             { "time_elapsed", Time.timeSinceLevelLoad }
//         });
//     }
// }
