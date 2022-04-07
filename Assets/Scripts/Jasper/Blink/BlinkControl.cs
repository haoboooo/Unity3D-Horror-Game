using System.Collections;
using UnityEngine;

public class BlinkControl : MonoBehaviour
{
    public static BlinkControl Instance { get; set; }

    public Shader blinkShader;
    private Material blinkMaterial;

    private float height = 0.0f;
    private float alpha = 0.0f;
    private bool useMaterial = false;

    public GameObject postProcessVolume;
    public GameObject tutorial;

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
        blinkMaterial = new Material(blinkShader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (useMaterial == false)
        {
            Graphics.Blit(source, destination);
            return;
        }

        blinkMaterial.SetFloat("_height", height);
        blinkMaterial.SetFloat("_alpha", alpha);
        Graphics.Blit(source, destination, blinkMaterial);
    }

    public void OpenEye()
    {
        StartCoroutine(StartOpenEye());
    }

    IEnumerator StartOpenEye()
    {
        useMaterial = true;
        alpha = 0.0f;
        height = 0.0f;

        yield return new WaitForSeconds(2.0f);
        GameControl.Instance.PlayPiano();
        yield return new WaitForSeconds(6.0f);

        float openEyeTime, closeEyeTime, openStayTime, closeStayTime, targetHeight, currTimer;
        {
            openEyeTime = 0.8f;
            closeEyeTime = 0.8f;
            openStayTime = 1.0f;
            closeStayTime = 1.0f;
            targetHeight = 0.2f;
            postProcessVolume.SetActive(true);

            currTimer = 0.0f;
            while (currTimer <= openEyeTime)
            {
                currTimer += Time.deltaTime;
                height = currTimer / openEyeTime * targetHeight;
                yield return null;
            }
            yield return new WaitForSeconds(openStayTime);

            currTimer = closeEyeTime;
            while (currTimer >= 0.0f)
            {
                currTimer -= Time.deltaTime;
                height = currTimer / closeEyeTime * targetHeight;
                yield return null;
            }
            yield return new WaitForSeconds(closeStayTime);

            postProcessVolume.SetActive(false);
        }

        {
            openEyeTime = 0.5f;
            closeEyeTime = 0.5f;
            openStayTime = 2.0f;
            closeStayTime = 0.3f;
            targetHeight = 0.33f;
            postProcessVolume.SetActive(true);

            currTimer = 0.0f;
            while (currTimer <= openEyeTime)
            {
                currTimer += Time.deltaTime;
                height = currTimer / openEyeTime * targetHeight;
                yield return null;
            }

            yield return new WaitForSeconds(openStayTime);

            currTimer = closeEyeTime;
            while (currTimer >= 0.0f)
            {
                currTimer -= Time.deltaTime;
                height = currTimer / closeEyeTime * targetHeight;
                yield return null;
            }
            yield return new WaitForSeconds(closeStayTime);

            postProcessVolume.SetActive(false);
        }

        {
            openEyeTime = 1.5f;
            targetHeight = 0.4f;

            currTimer = 0.0f;
            while (currTimer <= openEyeTime)
            {
                currTimer += Time.deltaTime;
                height = currTimer / openEyeTime * targetHeight;
                yield return null;
            }
            currTimer = 0.0f;
            while (currTimer <= openEyeTime)
            {
                currTimer += Time.deltaTime;
                alpha = currTimer / openEyeTime;
                yield return null;
            }
        }

        PlayerControl.Instance.SetCrossHair(true);
        PlayerControl.Instance.TurnOnControl();
        tutorial.SetActive(true);
        useMaterial = false;
        enabled = false;
    }

    public void CloseEye()
    {
        StartCoroutine(StartCloseEye());
    }

    IEnumerator StartCloseEye()
    {
        PlayerControl.Instance.TurnOffControl();
        yield return new WaitForSeconds(2.0f);

        PlayerControl.Instance.SetCrossHair(false);

        float closeEyeTime = 2.0f;
        float currTimer = closeEyeTime;
        float targetHeight = 0.4f;
        height = targetHeight;
        useMaterial = true;
        while (currTimer >= 0.0f)
        {
            currTimer -= Time.deltaTime;
            alpha = currTimer / closeEyeTime;
            yield return null;
        }
        currTimer = closeEyeTime;
        while (currTimer >= 0.0f)
        {
            currTimer -= Time.deltaTime;
            height = currTimer / closeEyeTime * targetHeight;
            yield return null;
        }
        GameControl.Instance.GameEnd();
    }
}
