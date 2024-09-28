using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public static GameOver instance;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject lostPanel;
    [SerializeField] TMP_Text scoreText;

    public bool isGameOver { get; private set; }
    void Awake() {
        instance = this;
    }

    public void Win() {
        winPanel.SetActive(true);
    }
    public void Lost() {
        isGameOver = true;
        lostPanel.SetActive(true);
    }
    public void TryAgain() {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu() {
        SceneManager.LoadScene(0);
    }
}
