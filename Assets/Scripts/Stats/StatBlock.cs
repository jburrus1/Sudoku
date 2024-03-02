using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatBlock : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _difficultyText;
    [SerializeField]
    private TextMeshProUGUI _gamesWon;
    [SerializeField]
    private TextMeshProUGUI _bestTime;
    [SerializeField]
    private TextMeshProUGUI _averageTime;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(DataModel.Stats stats)
    {
        var bestTS = TimeSpan.FromSeconds(stats.NumberOfWins == 0 ? 0 : stats.BestTime);
        var avgTS = TimeSpan.FromSeconds(stats.NumberOfWins == 0 ? 0 : stats.AverageTime);
        _difficultyText.text = stats._Difficulty.ToString();
        _gamesWon.text = $"Games Won:\n{stats.NumberOfWins}";
        _bestTime.text = $"Best Time:\n{(stats.NumberOfWins == 0 ? "N/A" : $"{bestTS.TotalMinutes:00}:{bestTS.Seconds:00}")}";
        _averageTime.text = $"Avg Time (Last 10):\n{(stats.NumberOfWins == 0 ? "N/A" : $"{avgTS.TotalMinutes:00}:{avgTS.Seconds:00}")}";
    }
}
