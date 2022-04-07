using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Check this box if you want this object to be directly interacted without any condition")]
    public bool noConditionNeed = false;
    //[Tooltip("Uncheck this box if this object needs to be unlocked before real interaction")]
    protected bool solvedPreLock = true;

    [Tooltip("Max distance that player can interact with this object")]
    public float maxInteractableDistance = 3.5f;

    [Header("Dialogue Settings")]
    public bool needDialogue = false;
    [ConditionalHide("needDialogue", true)] [TextArea]
    public string message;
    [ConditionalHide("needDialogue", true)]
    public bool onlyShowWhenLocked = false;

    [Header("Hint Settings")]
    public bool hasHint = false;
    [ConditionalHide("hasHint", true)]
    public List<Renderer> hintObjects;
    protected int myID;

    protected Collider myCollider;

    [Header("Bool Variables")]
    protected bool alreadyInteracted = false;
    protected bool alreadyHovered = false;
    protected bool meetInteractCondition = false;
    protected bool alreadyAddedHintObjects = false;
    protected bool currentlyMeetInteractionCondition = false;
    [HideInInspector]public bool canQuit = true;

    protected PlayerControl playerControl;

    public virtual void Start()
    {
        playerControl = PlayerControl.Instance;
        myID = gameObject.GetInstanceID();

        myCollider = GetComponent<Collider>();
        if (myCollider == null)
        {
            Debug.LogError(transform.name + " needs a collider in order to use interactable");
        }

        if (noConditionNeed == true)
        {
            meetInteractCondition = true;
        }
    }

    public void Update()
    {
        currentlyMeetInteractionCondition = meetInteractCondition & !playerControl.IsShowingHint;
        alreadyHovered &= !(playerControl.shiftUp || playerControl.shiftDown);

        float distanceWithPlayer = Vector3.Distance(playerControl.transform.position, transform.position);
        if (distanceWithPlayer < maxInteractableDistance)
        {
            RaycastHit hit;
            if (myCollider.Raycast(playerControl.rayFromScreenCenter, out hit, maxInteractableDistance * 3.0f))
            {
                if (alreadyHovered == false && alreadyInteracted == false)
                {
                    alreadyHovered = true;
                    if (currentlyMeetInteractionCondition)
                    {
                        playerControl.SetHandIcon(true);
                    }
                    else
                    {
                        playerControl.SetLockIcon(true);
                    }
                    if (hasHint && playerControl.IsShowingHint == false && playerControl.currentHintID != myID && (meetInteractCondition == false || solvedPreLock == false))
                    {
                        playerControl.SetHintUI(true);
                    }
                }

                if (Input.GetMouseButtonDown(0) && alreadyInteracted == false && InspectionSystem.Instance.IsInspecting == false)
                {
                    if (currentlyMeetInteractionCondition)
                    {
                        playerControl.interactTimes += 1;
                        Interact();
                    }
                    if (needDialogue)
                    {
                        if ((onlyShowWhenLocked == true && (meetInteractCondition == false || solvedPreLock == false)) || (onlyShowWhenLocked == false))
                        {
                            playerControl.ShowDialogue(message);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.H) && hasHint == true && playerControl.currentHintID != myID && playerControl.IsShowingHint == false)
                {
                    playerControl.WriteHintObjects(hintObjects);
                    alreadyAddedHintObjects = true;
                    playerControl.SetHintUI(false);
                    playerControl.currentHintID = myID;
                }
            }
            else
            {
                if (alreadyHovered == true)
                {
                    TurnOffAllHoverUI();
                }
                alreadyAddedHintObjects = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && alreadyInteracted == true && canQuit == true && Time.timeScale != 0.0f)
            {
                QuitInteracting();
            }
        }
        else
        {
            if (alreadyHovered == true)
            {
                TurnOffAllHoverUI();
            }
            alreadyAddedHintObjects = false;
        }
    }

    private void TurnOffAllHoverUI()
    {
        alreadyHovered = false;
        playerControl.SetHandIcon(false);
        playerControl.SetLockIcon(false);
        playerControl.SetHintUI(false);
    }

    public void Lock()
    {
        if (alreadyHovered)
        {
            playerControl.SetHandIcon(false);
            playerControl.SetLockIcon(true);
        }
        meetInteractCondition = false;
    }

    public void Unlock()
    {
        if (alreadyHovered)
        {
            playerControl.SetHandIcon(true);
            playerControl.SetLockIcon(false);
        }
        meetInteractCondition = true;
    }

    public virtual void Interact()
    {
        alreadyInteracted = true;
        playerControl.isInteractingWithObjects = true;
    }

    private void OnDisable()
    {
        playerControl.isInteractingWithObjects = false;
    }

    public virtual void FinishInteracting() // This is called when finish puzzle
    {
        ShutDown();
    }

    public virtual void QuitInteracting() // This is called when press space
    {
        ShutDown();
    }

    public void ShutDown()
    {
        alreadyInteracted = false;
        alreadyHovered = false;
        playerControl.isInteractingWithObjects = false;
    }

    public void DisablePlayerMovement()
    {
        playerControl.playerMovement.StopMove();
    }

    public void EnablePlayerMovement()
    {
        playerControl.playerMovement.StartMove();
    }
}