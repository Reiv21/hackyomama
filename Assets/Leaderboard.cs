using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Leaderboard : MonoBehaviour
{
    int missionNumber = -1;
    string[] players;
    int[] scores;
    [SerializeField] TMP_Text[] texts;
    [SerializeField] TMP_Text mainText;
    void Start()
    {
        mainText.text = "LEADERBOARD\nMission " + missionNumber;
        LoadPlayerData(10);
        RefreshLeaderboard();
        AddPlayers("TEST2", UnityEngine.Random.Range(1,100));
        SavePlayerData(players, scores);
        //LoadPlayerData(10);
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        for(int i = 0; i < players.Length; i++)
        {
            texts[i].text = i + 1 + ". " + players[i] + " - " + scores[i];
        }
    }
    public void AddPlayers(string newPlayer, int newScore)
    {
        List<string> playerList = new List<string>(players);
        List<int> scoreList = new List<int>(scores);
        playerList.Add(newPlayer);
        scoreList.Add(newScore);
        List<(string playerName, int playerScore)> playerScoreList = new List<(string, int)>();

        for (int i = 0; i < playerList.Count; i++)
        {
            playerScoreList.Add((playerList[i], scoreList[i]));
        }
        
        playerScoreList.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));
        playerList.RemoveAt(playerList.Count - 1);
        scoreList.RemoveAt(scoreList.Count - 1);
        players = playerList.ToArray();
        scores = scoreList.ToArray();
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] == null) continue;
            players[i] = playerScoreList[i].playerName;
            scores[i] = playerScoreList[i].playerScore;
        }
    }

    public void SavePlayerData(string[] playerNames, int[] playerScores)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            PlayerPrefs.SetString(missionNumber + "_PlayerName_" + i, playerNames[i]);
            
            PlayerPrefs.SetInt(missionNumber + "_PlayerScore_" + i, playerScores[i]);
        }
        
        PlayerPrefs.Save();
    }
    
    public void LoadPlayerData(int numberOfPlayers)
    {
        players = new string[10];
        scores = new int[10];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if(PlayerPrefs.GetString(missionNumber + "_PlayerName_" + i, "NULL") == "NULL") continue;
            
             players[i] = PlayerPrefs.GetString(missionNumber + "_PlayerName_" + i, "NULL");
             scores[i] = PlayerPrefs.GetInt(missionNumber + "_PlayerScore_" + i, -1);
        }
    }
}
