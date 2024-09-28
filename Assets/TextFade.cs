using System.Collections;
using UnityEngine;
using TMPro;
using static Level;

public class TextFade : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Assign the TMP text object in the Inspector
    public string newText;              // The new text to fade in
    public float fadeDuration = 1.0f;   // Duration of fade in/out
    public float initialDelay = 4.0f;   // Time to wait before starting fade out

    private void Start()
    {
        // Optionally call the FadeText function to trigger it automatically
        StartCoroutine(FadeTextOutIn(newText));
    }

    // Coroutine to fade out the current text and fade in the new text
    IEnumerator FadeTextOutIn(string nextText)
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);

        // Fade out current text
        yield return StartCoroutine(FadeOut());

        // Set new text
        textMeshPro.text = nextText;

        // Fade in new text
        yield return StartCoroutine(FadeIn());
    }

    // Coroutine to fade out the current text
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = textMeshPro.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMeshPro.color = color;
            yield return null;
        }

        // Make sure the text is fully transparent
        color.a = 0f;
        textMeshPro.color = color;
    }

    // Coroutine to fade in the new text
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = textMeshPro.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            textMeshPro.color = color;
            yield return null;
        }

        // Make sure the text is fully opaque
        color.a = 1f;
        textMeshPro.color = color;
    }
}
