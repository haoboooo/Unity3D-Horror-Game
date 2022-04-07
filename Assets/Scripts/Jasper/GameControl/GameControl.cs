using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance { get; set; }

    [Header("Basic Settings")]
    public GameObject Menu;
    public GameObject gameStartEffect;
    public bool hasGameStartEffect = true;

    [Header("Pause Settings")]
    private bool hasMouse = false;
    private bool isPaused = false;
    [HideInInspector]public bool canPause = true;

    [Header("Piano Music")]
    public AudioSource audioSource;
    public AudioClip pianoMusic;

    [Header("Game End Settings")]
    public GameObject gameEndUI;
    public Text gameEndText;

    private AudioSource playingAudio;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hasGameStartEffect == true)
        {
            gameStartEffect.SetActive(true);
            canPause = false;
        }
        else
        {
            PlayerControl.Instance.SetCrossHair(true);
            PlayerControl.Instance.TurnOnControl();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false && canPause == true)
        {
            PauseGame();

            playingAudio = null;
            AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audio in sources)
            {
                if (audio.isPlaying == true)
                {
                    playingAudio = audio;
                    audio.Pause();
                }
            }
        }
    }

    public void PlayPiano()
    {
        audioSource.clip = pianoMusic;
        audioSource.Play();
    }

    public void ResumeGame()
    {
        if (hasMouse == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        Menu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

        if (playingAudio != null)
        {
            playingAudio.UnPause();
        }
    }

    public void PauseGame()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            hasMouse = false;
        }
        else
        {
            hasMouse = true;
        }
        
        Menu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void GameEnd()
    {
        StartCoroutine(StartEndGame());
    }

    IEnumerator StartEndGame()
    {
        canPause = false;
        yield return new WaitForSeconds(1.0f);

        gameEndUI.SetActive(true);
        float phase_1 = 2.0f;
        float phase_2 = 2.0f;
        float phase_3 = 2.0f;
        float currentTimer = 0.0f;
        Color textColor = gameEndText.color;
        while (currentTimer <= phase_1)
        {
            currentTimer += Time.deltaTime;
            textColor.a = currentTimer / phase_1;
            gameEndText.color = textColor;
            yield return null;
        }
        yield return new WaitForSeconds(phase_2);
        currentTimer = phase_3;
        while (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
            currentTimer -= Time.deltaTime;
            textColor.a = currentTimer / phase_3;
            gameEndText.color = textColor;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        PlayPiano();
        yield return new WaitForSeconds(6.0f);

        textColor.a = 1;
        gameEndText.color = textColor;
        gameEndText.text = "Game Ends";
        canPause = true; 
    }
}
