using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Collider))]
public class ScaleObjectInfo : MonoBehaviour
{
    public Vector3 onScalePosition;
    public Vector3 onScaleRotation;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public float weight;
    [HideInInspector]public bool isOnScale;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        isOnScale = false;
    }

    public void Place()
    {
        if (isOnScale == true)
        {
            OffScale();
        }
        else
        {
            OnScale();
        }
    }

    void OnScale()
    {
        transform.localPosition = onScalePosition;
        transform.localRotation = Quaternion.Euler(onScaleRotation);
        isOnScale = true;
    }

    void OffScale()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        isOnScale = false;
    }
}
