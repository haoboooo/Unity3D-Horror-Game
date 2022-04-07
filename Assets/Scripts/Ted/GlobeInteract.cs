using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GlobeInteract : Interactable
{
    [Header("GlobeInteract Settings")]
    public AnimationClip RotateAnimClip;
    private RotateController rotateControl;
    private Animation anim;
    
    AnalyticsResult ar;//for debug use
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        anim = GetComponent<Animation>();
        rotateControl = GetComponentInChildren<RotateController>();
        // for debug use
        ar = Analytics.CustomEvent("rotate_globe");
    }

    public override void Interact()
    {
        base.Interact();
        //rotateControl.rotateGlobe();
        anim.Play();
        ReportRotateGlobe();
        //for debug use, test if custom event works
        Debug.Log("rotate_globe_Result = " + ar.ToString());

        StartCoroutine(Rotating());
    }

    IEnumerator Rotating()
    {
        yield return new WaitForSeconds(RotateAnimClip.length);
        base.FinishInteracting();
    }

    public void ReportRotateGlobe(){
    AnalyticsEvent.Custom("rotate_globe", new Dictionary<string, object>
    {
        { "time_elapsed", Time.timeSinceLevelLoad }
    });
    }
}
