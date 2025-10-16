using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int totalScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("ScoreManager active");
    }

    public void AddScore(int points)
    {
        totalScore += points;
        Debug.Log($"AddScore() called — total score now = {totalScore}");
    }
}