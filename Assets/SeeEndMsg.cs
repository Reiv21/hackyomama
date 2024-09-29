using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Level;
using TMPro;
using UnityEngine.SceneManagement;

public class SeeEndMsg : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Assign the TMP text object in the Inspector

    public float fadeDuration = 1.0f;   // Duration of fade in/out
    public float initialDelay = 4.0f;   // Time to wait before starting fade out
    public GameObject panel; public string newText;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(true);
        StartCoroutine(FadeTextOutIn(newText));
    }

    IEnumerator FadeTextOutIn(string nextText)
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);

        // Fade out current text
        yield return StartCoroutine(FadeOut());

        panel.SetActive(false);
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
}
