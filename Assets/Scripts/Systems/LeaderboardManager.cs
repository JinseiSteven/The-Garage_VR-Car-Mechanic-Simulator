using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;


public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    private string filePath;
    private List<float> leaderboard;
    public TextMeshProUGUI leaderboardOutput;

    private void Awake()
    {
        CreateSingleton();
        filePath = Application.persistentDataPath + "/leaderboard.json";
        LoadLeaderboard();

        // Add test scores
        SaveTaskCompletion(UnityEngine.Random.Range(10f, 100f));
        SaveTaskCompletion(UnityEngine.Random.Range(10f, 100f));
        SaveTaskCompletion(UnityEngine.Random.Range(10f, 100f));

        // Write leaderboard data to the in-game leaderboard
        WriteLeaderboard();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Leaderboard saved to {filePath}");
    }

    private void LoadLeaderboard()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            leaderboard = JsonUtility.FromJson<List<float>>(json);
        }
        else
        {
            leaderboard = new List<float>();
            SaveLeaderboard();
        }
    }

    private void WriteLeaderboard()
    {
        // Gather task names and format them as column headers
        string leaderboardText = "Storytime leaderboard (mm:ss:ms):\n";

        // Display top 5 times
        for (int i = 0; i < 5; i++)
        {
            if (leaderboard.Count > i)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(leaderboard[i]);
                leaderboardText += $"{i + 1}. {string.Format("{0:D2}:{1:D2}.{2:D3}\n", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds)}";
            }
            else
            {
                leaderboardText += $"{i + 1}. N/A\n";
            }
        }

        leaderboardText += "\nComplete the story time to get on the leaderboard!";

        // Update UI text
        leaderboardOutput.text = leaderboardText;
    }

    public void SaveTaskCompletion(float time)
    {
        // Add score and sort list
        leaderboard.Add(time);
        leaderboard.Sort();

        SaveLeaderboard();
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
