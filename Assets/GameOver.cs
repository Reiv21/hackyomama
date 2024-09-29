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

    public int CalcualteScore() {
        int score = 0;
        score += (LevelManager.instance.houseCount) * 100 / LevelManager.instance.housesStart;
        score += (int)(BuildingManager.instance.money * 2f);
        return score;
    }

    public bool isGameOver { get; private set; }
    void Awake() {
        instance = this;
    }

    public void Win() {
        winPanel.SetActive(true);
        scoreText.text = "Score: " + CalcualteScore();
        Leaderboard.instance.ShowLeaderboard();
        Leaderboard.instance.AddPlayers(DateTime.Now.ToString("yyyy-MM-dd-HH:mm"), CalcualteScore());
        Leaderboard.instance.SavePlayerData();
    }
    public void Lost() {
        isGameOver = true;
        lostPanel.SetActive(true);
    }
    public void TryAgain() {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Next() {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetSceneByName(SceneManager.GetActiveScene().name).buildIndex + 1);
    }
    public void Menu() {
        SceneManager.LoadScene(0);
    }
}
