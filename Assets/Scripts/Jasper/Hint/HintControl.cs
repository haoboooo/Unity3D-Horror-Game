using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintControl : MonoBehaviour
{
    [Header("Shaders")]
    public Shader hintShader;
    public Shader bloomShader;
    public Shader additiveShader;

    [Header("Materials")]
    public Material targetMaterial;
    public Material highlightMaterial;
    public Material hintMaterial;
    public Material highlightStartMateial;
    private Material grayScaleMaterial;
    private Material bloomMaterial;
    private Material additiveMaterial;

    [HideInInspector] public List<Renderer> hintObjects = new List<Renderer>();
    [HideInInspector] public List<Material> hintObjectOriginalMaterials = new List<Material>();

    [Header("Time Setting")]
    public float paddingTime = 0.0f;
    private float intensity = 1.0f;
    private float currTimer = 0.0f;

    [Header("Mode Setting")]
    public Text text;
    private bool hintIsInBag = false;

    [Header("Bloom Setting")]
    [Range(0.0f, 0.2f)] public float BlurIntensity = 0.0f;
    private int bloomMaskPass = 0;
    private int yAxisGuassianBlurPass = 1;
    private int xAxisGuassianBlurPass = 2;

    [Header("Colors")]
    private Color currHighlightColor;
    private float startHighlightRedValue;
    private float endHighlightRedValue;

    private IEnumerator enableCouroutine = null;
    private IEnumerator disableCouroutine = null;

    void Awake()
    {
        grayScaleMaterial = new Material(hintShader);
        grayScaleMaterial.SetColor("_TargetColor", targetMaterial.color);

        bloomMaterial = new Material(bloomShader);

        additiveMaterial = new Material(additiveShader);

        currHighlightColor = highlightMaterial.color;
        startHighlightRedValue = highlightStartMateial.color.r;
        endHighlightRedValue = currHighlightColor.r;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width;
        int height = source.height;
        RenderTexture grayScaleTexture = RenderTexture.GetTemporary(width, height);
        RenderTexture bloomMaskTexture = RenderTexture.GetTemporary(width, height);
        RenderTexture yAxisGaussianBlurTexture = RenderTexture.GetTemporary(width, height);
        RenderTexture finalAxisGaussianBlurTexture = RenderTexture.GetTemporary(width, height);

        if (intensity <= 1.0f)
        {
            grayScaleMaterial.SetFloat("_bwBlend", intensity);
            currHighlightColor.r = Mathf.Lerp(startHighlightRedValue, endHighlightRedValue, intensity);
            grayScaleMaterial.SetColor("_HighlightColor", currHighlightColor);
            bloomMaterial.SetColor("_HighlightColor", currHighlightColor);
            additiveMaterial.SetColor("_HighlightColor", currHighlightColor);
        }

        Graphics.Blit(source, grayScaleTexture, grayScaleMaterial);

        bloomMaterial.SetFloat("_BlurSize", BlurIntensity);
        Graphics.Blit(grayScaleTexture, bloomMaskTexture, bloomMaterial, bloomMaskPass);
        Graphics.Blit(bloomMaskTexture, yAxisGaussianBlurTexture, bloomMaterial, yAxisGuassianBlurPass);
        Graphics.Blit(yAxisGaussianBlurTexture, finalAxisGaussianBlurTexture, bloomMaterial, xAxisGuassianBlurPass);

        additiveMaterial.SetTexture("_BloomTex", finalAxisGaussianBlurTexture);
        Graphics.Blit(grayScaleTexture, destination, additiveMaterial);

        RenderTexture.ReleaseTemporary(grayScaleTexture);
        RenderTexture.ReleaseTemporary(bloomMaskTexture);
        RenderTexture.ReleaseTemporary(yAxisGaussianBlurTexture);
        RenderTexture.ReleaseTemporary(finalAxisGaussianBlurTexture);
    }

    private void OnEnable()
    {
        intensity = 0.0f;
        currTimer = 0.0f;

        if (hintObjects.Count > 0 && hintObjects[0].gameObject.activeInHierarchy == false)
        {
            hintIsInBag = true;
            text.text = "Hint is the " + hintObjects[0].gameObject.name + " in the bag";
            text.gameObject.SetActive(true);
        }
        else
        {
            hintIsInBag = false ;
            for (int i = 0; i < hintObjects.Count; i++)
            {
                hintObjects[i].material = hintMaterial;
            }
        }

        if (disableCouroutine != null)
        {
            StopCoroutine(disableCouroutine);
        }
        enableCouroutine = TurnOnHint();
        StartCoroutine(enableCouroutine);
    }

    private void OnDisable()
    {
        if (hintIsInBag)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < hintObjects.Count; i++)
            {
                if (hintObjectOriginalMaterials[i] == null)
                {
                    Debug.LogError("Missing original materials");
                }
                hintObjects[i].material = hintObjectOriginalMaterials[i];
            }
        }
    }

    IEnumerator TurnOnHint()
    {
        while (currTimer < paddingTime)
        {
            currTimer += Time.deltaTime;
            intensity = currTimer / paddingTime;
            yield return null;
        }
    }

    public void TurnOff()
    {
        currTimer = paddingTime;

        if (enableCouroutine != null)
        {
            StopCoroutine(enableCouroutine);
        }
        disableCouroutine = TurnOffHint();
        StartCoroutine(disableCouroutine);
    }

    IEnumerator TurnOffHint()
    {
        while (currTimer > 0.0f)
        {
            currTimer -= Time.deltaTime;
            intensity = currTimer / paddingTime;
            yield return null;
        }
        enabled = false;
    }
}
