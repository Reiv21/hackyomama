using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainIntensity : MonoBehaviour
{
    public float value = 0f;  // The value to be incremented
    public float duration = 5f; // Time in seconds to complete the transition

    // Start the value increment process
    void Start()
    {
        StartCoroutine(IncreaseValueOverTime());
    }

    IEnumerator IncreaseValueOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            value = Mathf.Lerp(0f, 1f, elapsedTime / duration); // Interpolates from 0 to 1
            yield return null; // Wait for the next frame
        }

        // Ensure value is exactly 1 at the end
        value = 1f;
    }
}
