using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetControl : MonoBehaviour
{
    [Header("Interactable")]
    public HelmetInteractable helmetInteractable;
    public FirePlaceInteractable firePlaceInteractable;

    [Header("Helmet Move Settings")]
    public float rotateSpeed;
    public float horizontalLimit;
    public float verticalLimit;
    private Vector3 currentRotation;

    [Header("Laser Settings")]
    public LineRenderer lineRender;
    public Transform lineStartPoint;
    private Vector3 lineStartPosition;
    private int layerMask;

    [Header("Ignite Settings")]
    public Collider targetCollider;
    public GameObject fireEffect;
    public AudioSource fireAudio;
    public float targetFocusTime;
    private float onTargetTimer;
    private bool alreadyIgnited;

    [Header("Morse Code Settings")]
    public AudioClip shortClip;
    public AudioClip longClip;
    public string keyWord;
    private float letterPadding = 0.2f;
    private AudioSource morseAudioSource;
    private List<string> currentMorseCode;
    private Dictionary<char, string> morseCodeTable;
    private IEnumerator morseCoroutine;

    void Awake()
    {
        InitializeMorseCode();
        morseAudioSource = GetComponent<AudioSource>();
        morseCoroutine = null;
    }

    void Start()
    {
        currentRotation = transform.localRotation.eulerAngles;
        layerMask = ~(1 << 9);

        lineStartPosition = lineStartPoint.position;
        lineRender.SetPosition(0, lineStartPoint.localPosition);

        alreadyIgnited = false;
    }

    void Update()
    {
        ControlHelmet();
        DrawLine();
        CheckTargetCollider();
    }

    private void OnEnable()
    {
        morseCoroutine = PlayMorseCode();
        StartCoroutine(morseCoroutine);
    }

    private void OnDisable()
    {
        if (morseCoroutine != null)
        {
            StopCoroutine(morseCoroutine);
        }
    }

    void ControlHelmet()
    {
        float horizontalDelta = rotateSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        float verticalDelta = -rotateSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        currentRotation.x = Mathf.Clamp(currentRotation.x + verticalDelta, -verticalLimit, verticalLimit);
        currentRotation.y = Mathf.Clamp(currentRotation.y + horizontalDelta, -horizontalLimit, horizontalLimit);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    void DrawLine()
    {
        RaycastHit hit;
        Debug.DrawRay(lineStartPosition, transform.forward * 10);
        if (Physics.Raycast(lineStartPosition, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            lineRender.SetPosition(1, transform.InverseTransformPoint(hit.point));
        }
    }

    void CheckTargetCollider()
    {
        if (targetCollider.gameObject.activeInHierarchy == false || alreadyIgnited == true)
        {
            return;
        }

        RaycastHit hit;
        if (targetCollider.Raycast(new Ray(lineStartPosition, transform.forward), out hit, Mathf.Infinity))
        {
            onTargetTimer += Time.deltaTime;
        }
        else
        {
            onTargetTimer = 0.0f;
        }

        if (onTargetTimer > targetFocusTime)
        {
            StartCoroutine(Ignite());
        }
    }

    IEnumerator Ignite()
    {
        alreadyIgnited = true;
        firePlaceInteractable.Unlock();
        fireAudio.Play();
        if (morseCoroutine != null)
        {
            StopCoroutine(morseCoroutine);
        }

        yield return new WaitForSeconds(0.7f);

        fireEffect.SetActive(true);

        yield return new WaitForSeconds(2);

        lineRender.enabled = false;
        helmetInteractable.FinishInteracting();
    }

    IEnumerator PlayMorseCode()
    {
        float shortPadding = shortClip.length + letterPadding;
        float longPaddding = longClip.length + letterPadding;
        while (true)
        {
            foreach (string word in currentMorseCode)
            {
                foreach (char ch in word)
                {
                    if (ch == '.')
                    {
                        morseAudioSource.clip = shortClip;
                        morseAudioSource.Play();
                        yield return new WaitForSeconds(shortPadding);
                    }
                    else
                    {
                        morseAudioSource.clip = longClip;
                        morseAudioSource.Play();
                        yield return new WaitForSeconds(longPaddding);
                    }
                }
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    void InitializeMorseCode()
    {
        morseCodeTable = new Dictionary<char, string>();
        morseCodeTable.Add('a', "._");
        morseCodeTable.Add('b', "_...");
        morseCodeTable.Add('c', "_._.");
        morseCodeTable.Add('d', "_..");
        morseCodeTable.Add('e', ".");
        morseCodeTable.Add('f', ".._.");
        morseCodeTable.Add('g', "__.");
        morseCodeTable.Add('h', "....");
        morseCodeTable.Add('i', "..");
        morseCodeTable.Add('j', ".___");
        morseCodeTable.Add('k', "_._");
        morseCodeTable.Add('l', "._..");
        morseCodeTable.Add('m', "__");
        morseCodeTable.Add('n', "_.");
        morseCodeTable.Add('o', "___");
        morseCodeTable.Add('p', ".__.");
        morseCodeTable.Add('q', "__._");
        morseCodeTable.Add('r', "._.");
        morseCodeTable.Add('s', "...");
        morseCodeTable.Add('t', "_");
        morseCodeTable.Add('u', ".._");
        morseCodeTable.Add('v', "..._");
        morseCodeTable.Add('w', ".__");
        morseCodeTable.Add('x', "_.._");
        morseCodeTable.Add('y', "_.__");
        morseCodeTable.Add('z', "__..");

        currentMorseCode = new List<string>();
        foreach (char ch in keyWord)
        {
            currentMorseCode.Add(morseCodeTable[ch]);
        }
    }
}
