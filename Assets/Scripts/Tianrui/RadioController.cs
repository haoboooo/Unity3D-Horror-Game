using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    public GameObject TuningKnob;
    public GameObject Indicator;
    private AudioSource Audio1;
    private AudioSource Audio2;
    private AudioSource Audio3;
    private float KnobSpeed = -0.01f;
    private float IndicatorSpeed = 18f;
    private float KnobPosition = 0f;
    private float IndicatorPosition = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float BeforePosition = TuningKnob.transform.localEulerAngles.z;
        if (Input.GetKey(KeyCode.D))
        {
            if (Indicator.transform.localPosition.x >= -0.1f)
            {
                TuningKnob.transform.Rotate(0, 0, 0.18f);
                Vector3 vector = Indicator.transform.localPosition;
                Indicator.transform.localPosition = new Vector3(vector.x - 0.0001f, vector.y, vector.z);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Indicator.transform.localPosition.x <= 0.1f)
            {
                TuningKnob.transform.Rotate(0, 0, -0.18f);
                Vector3 vector = Indicator.transform.localPosition;
                Indicator.transform.localPosition = new Vector3(vector.x + 0.0001f, vector.y, vector.z);
            }
        }
        float AfterPosition = TuningKnob.transform.localEulerAngles.z;
        CheckPosition(BeforePosition, AfterPosition);

    }
    void CheckPosition(float before, float after)
    {
        if (before < 120 && after < 120)
        {
            //start audio1 if not started
            if (!Audio1.isPlaying)
            {
                Audio1.Play();
                Debug.Log("Play Audio1");
            }
        }
        else if(before < 120 && after >= 120)
        {
            //switch from audio1 to audio2
            if (!Audio1.isPlaying)
            {
                Audio2.Play();
                Debug.Log("Play Audio2");
            }
            else
            {
                Audio1.Stop();
                Audio2.Play();
                Debug.Log("Stop Audio1 and Play Audio2");
            }
        }
        else if(before < 240 && after >= 240)
        {
            //switch from audio2 to audio3
            if (!Audio2.isPlaying)
            {
                Audio3.Play();
                Debug.Log("Play Audio3");
            }
            else
            {
                Audio2.Stop();
                Audio3.Play();
                Debug.Log("Stop Audio2 and Play Audio3");
            }
        }
    }
}
