using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityAtoms.BaseAtoms;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] private VoidEvent DeathSequenceFinished;

    [Header("Fade Settings")]
    public float fadeSpeed = 1f; // Time in seconds for fade

    [Header("UI References")]
    public Image fadeImage; // Black image that covers screen

    [Header("Death text")]
    public TextMeshProUGUI deathText;

    private bool isFading = false;

    private Coroutine currentCoroutine;

    void Start()
    {
        // Start with transparent (no fade)
        SetAlpha(0f);
    }

    private void OnEnable()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        SetAlpha(0f);
    }

    private void OnDisable()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    // Fade to black
    [Button]
    public void FadeOut()
    {
        if (!isFading)
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            currentCoroutine = StartCoroutine(FadeCoroutine(0f, 1f));
        }
    }

    // Fade from black to clear
    [Button]
    public void FadeIn()
    {
        if (!isFading)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            currentCoroutine = StartCoroutine(FadeCoroutine(1f, 0f));
        }
    }

    [Button]
    // Fade out then fade back in
    public IEnumerator FadeOutAndIn()
    {
        yield return StartCoroutine(FadeCoroutine(0f, 1f));
        yield return new WaitForSeconds(0.5f); // Wait half second
        yield return StartCoroutine(FadeCoroutine(1f, 0f));
    }

    // Main fade coroutine
    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeSpeed);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(endAlpha);
        isFading = false;

        DeathSequenceFinished.Raise();
    }

    // Set the alpha of the fade image
    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }

        if (deathText != null)
        {
            Color color = deathText.color;
            color.a = alpha;
            deathText.color = color;
        }
    }

    // Public method to check if currently fading
    public bool IsFading()
    {
        return isFading;
    }
}