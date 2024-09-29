using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Name of the scene you want to load after 5 seconds
    public string sceneToLoad;

    void Start()
    {
        // Start the coroutine to wait 5 seconds and then load the scene
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
