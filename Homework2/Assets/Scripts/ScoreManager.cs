using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    
    private int _totalScore = 0;
    public static event Action<int> onScoreChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        _totalScore += points;
        onScoreChanged?.Invoke(_totalScore);
        Debug.Log($"Total score now: {_totalScore}");
    }

    public void ResetScore()
    {
        _totalScore = 0;
        onScoreChanged?.Invoke(_totalScore);
    }

    public int GetCurrentScore()
    {
        return _totalScore;
    }
}