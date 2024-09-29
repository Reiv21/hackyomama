using UnityEngine;

public class Jukebox : MonoBehaviour {
    public static Jukebox instance;

    private AudioSource audioSource;

    public AudioClip clickSound, place0Sound, place1Sound, waterSound, breakSound;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.Log("ClickAudio instance already assigned");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClick() {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayPlace0() {
        audioSource.PlayOneShot(place0Sound);
    }

    public void PlayPlace1() {
        audioSource.PlayOneShot(place1Sound);
    }

    public void PlayWater() {
        audioSource.PlayOneShot(waterSound);
    }

    public void PlayBreak() {
        audioSource.PlayOneShot(breakSound);
    }


}
