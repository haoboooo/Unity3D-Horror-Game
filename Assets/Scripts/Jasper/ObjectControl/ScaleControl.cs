using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleControl : MonoBehaviour
{
    [Header("Scale Settings")]
    public Transform indicator;
    public float indicatorRotationSpeed;
    public float maxWeight = 5.0f;
    public float correctWeight;
    public float currentWeight;
    private float currentRotation;
    private bool indiacatorIsRotating;

    [Header("Scale Object")]
    public List<Collider> ScaleObjects;

    [Header("Scale Interactable")]
    public ScaleInteractable scaleInteractable;

    private Camera cam;
    private int currentHoverIndex;
    private bool isHovering;
    private IEnumerator hoverCoroutine;

    void Start()
    {
        currentWeight = 0.0f;
        currentRotation = 0.0f;
        currentHoverIndex = -1;

        indiacatorIsRotating = false;
        isHovering = false;

        cam = PlayerControl.Instance.focusCamera.GetComponent<Camera>();
    }

    private void OnEnable()
    {
        hoverCoroutine = Hover();
        StartCoroutine(hoverCoroutine);
    }

    private void OnDisable()
    {
        if (currentHoverIndex >= 0)
        {
            ScaleObjects[currentHoverIndex].GetComponent<cakeslice.Outline>().enabled = false;
        }
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
    }

    void Update()
    {
        if (isHovering == true && indiacatorIsRotating == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ScaleObjectInfo currentScaleObject = ScaleObjects[currentHoverIndex].gameObject.GetComponent<ScaleObjectInfo>();
                currentScaleObject.Place();
                float deltaWeight = (currentScaleObject.isOnScale ? 1.0f : -1.0f) * currentScaleObject.weight;
                WeightChange(deltaWeight);

                isHovering = false;
                ScaleObjects[currentHoverIndex].GetComponent<cakeslice.Outline>().enabled = false;
                currentHoverIndex = -1;
            }
        }
    }

    IEnumerator Hover()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            for (int i = 0; i < ScaleObjects.Count; i++)
            {
                RaycastHit hitInfo;
                if (ScaleObjects[i].Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
                {
                    if (currentHoverIndex == i)
                    {
                        break;
                    }
                    else
                    {
                        isHovering = true;
                        if (currentHoverIndex > 0)
                        {
                            ScaleObjects[currentHoverIndex].GetComponent<cakeslice.Outline>().enabled = false;
                        }
                        ScaleObjects[i].GetComponent<cakeslice.Outline>().enabled = true;
                        currentHoverIndex = i;
                    }
                }
                else
                {
                    if (currentHoverIndex == i)
                    {
                        isHovering = false;
                        ScaleObjects[currentHoverIndex].GetComponent<cakeslice.Outline>().enabled = false;
                        currentHoverIndex = -1;
                    }
                }
            }
        }
    }

    void WeightChange(float deltaWeight)
    {
        float targetWeight = currentWeight + deltaWeight;
        if (targetWeight >= maxWeight && currentWeight <= maxWeight)
        {
            deltaWeight = maxWeight - currentWeight;
        }
        else if (targetWeight <= maxWeight && currentWeight >= maxWeight)
        {
            deltaWeight = targetWeight - maxWeight;
        }
        else if (targetWeight >= maxWeight && currentWeight >= maxWeight)
        {
            deltaWeight = 0.0f;
        }

        currentWeight = targetWeight;
        if (deltaWeight != 0.0f)
        {
            StartCoroutine(StartWeightChange(deltaWeight));
        }
    }

    IEnumerator StartWeightChange(float deltaWeight)
    {
        indiacatorIsRotating = true;

        float rotSpeed = (deltaWeight > 0.0f ? 1.0f : -1.0f) * indicatorRotationSpeed;
        float rotDiff = deltaWeight / maxWeight * 360.0f;
        float targetRot = rotDiff + currentRotation;
        rotDiff = Mathf.Abs(rotDiff);
        Vector3 rot = indicator.localRotation.eulerAngles;
        float deltaRot;
        while (rotDiff > 0.0f)
        {
            deltaRot = Time.deltaTime * rotSpeed;

            rotDiff -= Mathf.Abs(deltaRot);
            if (rotDiff <= 0.0f)
            {
                currentRotation = targetRot;
            }
            else
            {
                currentRotation += Time.deltaTime * rotSpeed;
            }  

            rot.z = currentRotation;
            indicator.localRotation = Quaternion.Euler(rot);

            yield return null;
        }

        indiacatorIsRotating = false;
        
        if (Mathf.Abs(correctWeight - currentWeight) < 0.01f)
        {
            yield return new WaitForSeconds(1);

            scaleInteractable.FinishInteracting();
        }
    }
}
