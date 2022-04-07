using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    public static ElevatorControl Instance { get; set; }

    [Header("Animation")]
    public Animation elevatorAnimation;
    public List<Animation> gateAnimations;
    private float doorAnimationTime = 2.0f;
    private float moveAnimationTime = 3.0f; 

    [Header("Three Floors")]
    public List<GameObject> Floors;

    [Header("Elevator Settings")]
    public Transform elevatorModel;
    public Collider airWall;
    public int currentFloorIndex = 3;

    [Header("Sound Settings")]
    public AudioSource elevatorAudio;
    public AudioClip doorSound;
    public AudioClip moveSound;
    public AudioClip ringSound;

    [Header("Button Interact")]
    public FunctionalInteractable upButton;
    public FunctionalInteractable downbutton;

    private HashSet<int> accessableFloors;
    private int direction;
    private IEnumerator movingCoroutine;

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
        accessableFloors = new HashSet<int>();
        accessableFloors.Add(3);
        accessableFloors.Add(2);
    }

    void Start()
    {
        direction = 0;
        movingCoroutine = null;
    }

    public void MakeFloorAccessable(int index)
    {
        accessableFloors.Add(index);
    }

    public void PressButton(bool up)
    {
        direction = up ? 1 : -1;
        int targetFloor = currentFloorIndex + direction;
        if (accessableFloors.Contains(targetFloor) == false)
        {
            return;
        }
        elevatorAudio.clip = doorSound;
        elevatorAudio.Play();
        gateAnimations[currentFloorIndex - 1].Play("Close");
        currentFloorIndex = targetFloor;
        airWall.enabled = true;

        upButton.Lock();
        downbutton.Lock();

        StartCoroutine(StartClose());
    }

    IEnumerator StartClose()
    {
        yield return new WaitForSeconds(doorAnimationTime);
        Move();

        yield return new WaitForSeconds(moveAnimationTime);
        arriveFloor();

        yield return new WaitForSeconds(doorAnimationTime);
        FinishOpenDoor();
    }

    public void Move()
    {
        StartCoroutine(StartMove());
    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(0.5f);

        elevatorAudio.clip = moveSound;
        elevatorAudio.Play();

        if (direction > 0)
        {
            if (currentFloorIndex == 2)
            {
                elevatorAnimation.Play("One_Two");
            }
            else
            {
                elevatorAnimation.Play("Two_Three");
            }
        }
        else
        {
            if (currentFloorIndex == 2)
            {
                elevatorAnimation.Play("Three_Two");
            }
            else
            {
                elevatorAnimation.Play("Two_One");
            }
        }

        Floors[currentFloorIndex - 1].SetActive(true);

        movingCoroutine = UpdatePlayerPosition();
        StartCoroutine(movingCoroutine);
    }

    IEnumerator UpdatePlayerPosition()
    {
        float lastY = elevatorModel.position.y;
        float currentY;
        float diffY;
        Transform player = PlayerControl.Instance.transform;
        Vector3 playerPos = player.position;
        float playerY = playerPos.y;
        while (true)
        {
            currentY = elevatorModel.position.y;
            diffY = currentY - lastY;
            playerY += diffY;
            playerPos.y = playerY;
            player.position = playerPos;
            yield return null;

            lastY = currentY;
        }
    }

    public void arriveFloor()
    {
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        elevatorAudio.clip = ringSound;
        elevatorAudio.Play();

        yield return new WaitForSeconds(1.5f);

        elevatorAudio.clip = doorSound;
        elevatorAudio.Play();

        gateAnimations[currentFloorIndex - 1].Play("Open");
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
        }

        int lastFloor = currentFloorIndex + direction * -1;
 
        if (accessableFloors.Contains(lastFloor))
        {
            Floors[lastFloor - 1].SetActive(false);
        }
        else
        {
            Debug.LogError(lastFloor + " is not accessable");
        }
    }

    public void FinishOpenDoor()
    {
        airWall.enabled = false;

        upButton.Unlock();
        downbutton.Unlock();
    }
}
