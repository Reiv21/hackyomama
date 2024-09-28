using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Level;
using TMPro;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public AudioClip audioClip; // Assign the audio clip in the Inspector
    public Image imageToShow1;   // Assign the UI image in the Inspector
    public Image imageToShow2;   // Assign the UI image in the Inspector
    public float delay = 0.5f;  // Time to wait between repetitions

    private AudioSource audioSource;
    public AudioSource musicSource;
    public AudioSource rainSource;

    public TextMeshProUGUI textMeshPro; // Assign the TMP text object in the Inspector
    public string newText;              // The new text to fade in
    public float fadeDuration = 1.0f;   // Duration of fade in/out
    public float initialDelay = 4.0f;   // Time to wait before starting fade out


    public GameObject panel;
    void Start()
    {
        imageToShow1.enabled = false;
        imageToShow2.enabled = false;
        panel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
    }

    public void PlayGame()
    {
        StartCoroutine(PlayAudioAndShowImage());
    }

    IEnumerator PlayAudioAndShowImage()
    {
        musicSource.volume = 0.1f;
        audioSource.clip = audioClip;
        audioSource.pitch = 0.5f;
        audioSource.Play();

        // Show the image
        imageToShow1.enabled = true;

        // Wait for the audio to finish or half a second, whichever is longer
        yield return new WaitForSeconds(audioClip.length > 0.5f ? audioClip.length : 0.5f);

        audioSource.Play();
        imageToShow2.enabled = true;
            
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        panel.SetActive(true);
        StartCoroutine(FadeTextOutIn(newText));
        rainSource.Play();
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

        SceneManager.LoadScene("Cutscene");
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
