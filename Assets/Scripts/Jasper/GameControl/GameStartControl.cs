using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartControl : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backGround;
    public Text usc, games;
    public Text logo;
    private Color uscColor, gamesColor, logoColor;

    private Kino.AnalogGlitch analogGlitch;
    private Kino.DigitalGlitch digitalGlitch;

    void Start()
    {
        analogGlitch = GetComponentInChildren<Kino.AnalogGlitch>();
        digitalGlitch = GetComponentInChildren<Kino.DigitalGlitch>();
        analogGlitch.enabled = false;
        digitalGlitch.enabled = false;
        
        uscColor = usc.color;
        gamesColor = games.color;
        logoColor = logo.color;

        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        PlayerControl.Instance.playerCamera.GetComponent<Camera>().enabled = false;

        float currentTimer = 0.0f;
        float textEmergeTime = 4.0f;
        while (currentTimer <= textEmergeTime)
        {
            currentTimer += Time.deltaTime;
            uscColor.a = currentTimer / textEmergeTime;
            gamesColor.a = uscColor.a;
            usc.color = uscColor;
            games.color = gamesColor;
            yield return null;
        }

        analogGlitch.enabled = true;
        float phase_1 = 1.2f;
        float phase_2 = 0.5f;
        float phase_3 = 1.2f;
        float analogIntensity = 0.3f;
        currentTimer = 0.0f;
        while (currentTimer <= phase_1)
        {
            currentTimer += Time.deltaTime;
            analogGlitch.colorDrift = currentTimer / phase_1 * analogIntensity;
            analogGlitch.scanLineJitter = currentTimer / phase_1 * analogIntensity * 2.0f;
            analogGlitch.horizontalShake = currentTimer / phase_1 * analogIntensity;
            yield return null;
        }
        yield return new WaitForSeconds(phase_2);
        currentTimer = phase_3;
        while (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
            analogGlitch.colorDrift = currentTimer / phase_3 * analogIntensity;
            analogGlitch.scanLineJitter = currentTimer / phase_3 * analogIntensity * 2.0f;
            analogGlitch.horizontalShake = currentTimer / phase_3 * analogIntensity;
            yield return null;
        }
        analogGlitch.enabled = false;

        yield return new WaitForSeconds(1.0f);

        float digitalTime = 2.0f;
        float alpha;
        currentTimer = digitalTime;
        Color backGroundColor = backGround.color;
        while (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
            alpha = currentTimer / digitalTime;

            uscColor.a = alpha;
            gamesColor.a = alpha;
            usc.color = uscColor;
            games.color = gamesColor;

            backGroundColor.r = alpha;
            backGroundColor.g = alpha;
            backGroundColor.b = alpha;
            backGround.color = backGroundColor;

            yield return null;
        }
        digitalGlitch.enabled = false;
        usc.gameObject.SetActive(false);
        games.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        phase_1 = 2.0f;
        phase_2 = 2.0f;
        phase_3 = 2.0f;
        currentTimer = 0.0f;
        logo.gameObject.SetActive(true);
        while (currentTimer <= phase_1)
        {
            currentTimer += Time.deltaTime;
            logoColor.a = currentTimer / phase_1;
            logo.color = logoColor;
            yield return null;
        }
        yield return new WaitForSeconds(phase_2);
        currentTimer = phase_3;
        digitalGlitch.enabled = true;
        digitalGlitch.intensity = 0.5f;
        while (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
            logoColor.a = currentTimer / phase_1;
            logo.color = logoColor;
            yield return null;
        }
        digitalGlitch.enabled = false;

        BlinkControl.Instance.enabled = true;
        BlinkControl.Instance.OpenEye();
        PlayerControl.Instance.playerCamera.GetComponent<Camera>().enabled = true;
        gameObject.SetActive(false);
        GameControl.Instance.canPause = true;
    }
}
