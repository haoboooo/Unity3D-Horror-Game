using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionSystem : MonoBehaviour
{
    public GameObject canvas;
    public GameObject camera;
    public GameObject volume;
    public GameObject light;

    public static InspectionSystem Instance { get; set; }
    [HideInInspector] public bool IsInspecting = false;

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

    public void TurnOn()
    {
        IsInspecting = true;
        PlayerControl.Instance.playerMovement.StopMove();
        Cursor.lockState = CursorLockMode.None;
        canvas.SetActive(true);
        camera.SetActive(true);
        camera.GetComponent<AudioListener>().enabled = true;
        PlayerControl.Instance.playerCamera.GetComponent<AudioListener>().enabled = false;
        volume.SetActive(true);
        light.SetActive(true);
        PlayerControl.Instance.TurnOffInteractableHoverUI();
    }

    public void TurnOff()
    {
        IsInspecting = false;
        PlayerControl.Instance.playerMovement.StartMove();
        Cursor.lockState = CursorLockMode.Locked;
        canvas.SetActive(false);

        camera.SetActive(false);
        camera.GetComponent<AudioListener>().enabled = false;
        PlayerControl.Instance.playerCamera.GetComponent<AudioListener>().enabled = true;

        volume.SetActive(false);
        light.SetActive(false);
        PlayerControl.Instance.TurnOnInteractableHoverUI();
    }
}
