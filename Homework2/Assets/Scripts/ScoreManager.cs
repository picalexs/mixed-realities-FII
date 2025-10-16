using UnityEngine;
using UnityEngine.UI; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Text scoreText; 
    private int totalScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (scoreText) scoreText.text = "Score: 0";
    }

    public void AddScore(int points)
    {
        totalScore += points;
        if (scoreText) scoreText.text = "Score: " + totalScore;
        Debug.Log($"Scor total acum: {totalScore}");
    }
}