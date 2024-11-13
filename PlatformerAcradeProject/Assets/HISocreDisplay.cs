using System.IO;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class HiScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string leaderboardFilePath;
    private long highestScore;

    // Helper classes to represent the JSON structure
    [System.Serializable]
    private class ScoreEntry
    {
        public string name;
        public long score;
    }

    [System.Serializable]
    private class LeaderboardData
    {
        public List<ScoreEntry> entries;
    }

    void Start()
    {
        leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        highestScore = GetHighestScoreFromFile();
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        text.text = "High Score: " + highestScore;
    }

    long GetHighestScoreFromFile()
    {
        if (!File.Exists(leaderboardFilePath))
        {
            Debug.LogWarning("Leaderboard file not found.");
            return 0; // Return 0 if no file exists
        }

        string json = File.ReadAllText(leaderboardFilePath);

        try
        {
            LeaderboardData leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);
            
            if (leaderboardData != null && leaderboardData.entries != null && leaderboardData.entries.Count > 0)
            {
                long highestScore = 0;
                
                foreach (var entry in leaderboardData.entries)
                {
                    if (entry.score > highestScore)
                    {
                        highestScore = entry.score;
                    }
                }

                return highestScore;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading leaderboard JSON: " + e.Message);
        }

        return 0;
    }
}



