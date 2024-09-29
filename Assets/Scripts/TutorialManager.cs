using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
    public TMP_Text continueText;

    void Update() {
        var timeSin = Mathf.Sin(Time.time);
        continueText.color = Color.Lerp(new Color(0.6f, 0.0f, 0.0f), Color.red, timeSin);

        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("Lvl1");
        }
    }
}
