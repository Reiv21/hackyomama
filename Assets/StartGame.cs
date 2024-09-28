using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public AudioClip audioClip; // Assign the audio clip in the Inspector
    public Image imageToShow1;   // Assign the UI image in the Inspector
    public Image imageToShow2;   // Assign the UI image in the Inspector
    public float delay = 0.5f;  // Time to wait between repetitions

    private AudioSource audioSource;

    void Start()
    {
        imageToShow1.enabled = false;
        imageToShow2.enabled = false;
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

            audioSource.clip = audioClip;
            audioSource.Play();

            // Show the image
            imageToShow1.enabled = true;

            // Wait for the audio to finish or half a second, whichever is longer
            yield return new WaitForSeconds(audioClip.length > 0.5f ? audioClip.length : 0.5f);

            audioSource.Play();
            imageToShow2.enabled = true;
            
            // Wait for the specified delay
            yield return new WaitForSeconds(delay);

    }
}
