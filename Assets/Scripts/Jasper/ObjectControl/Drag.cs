using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [Tooltip("Check this box if you need to manually fix the pivot position")]
    public bool fixPivot = false;
    [ConditionalHide("fixPivot", true)]
    public Transform pivotTransform;

    public float LightIntensity = 1.0f;

    private Vector3 lastPos, currPos;
    private float rotationSpeed = -0.2f;
    private Vector3 pivotPosition;

    void Start()
    {
        lastPos = Input.mousePosition;
        pivotPosition = fixPivot ? pivotTransform.position : transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currPos = Input.mousePosition;
            Vector3 offset = currPos - lastPos;
            transform.RotateAround(pivotPosition, Vector3.up, offset.x * rotationSpeed);
            transform.RotateAround(pivotPosition, Vector3.forward, offset.y * -rotationSpeed);
        }
        lastPos = Input.mousePosition;
    }

    private void OnEnable()
    {
        InspectionSystem.Instance.light.GetComponent<Light>().intensity = LightIntensity;
        lastPos = Input.mousePosition;
    }
}
