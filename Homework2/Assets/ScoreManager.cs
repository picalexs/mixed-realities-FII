using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int totalScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Debug.Log("✅ ScoreManager s-a inițializat corect (Awake)");
    }

    void Start()
    {
        Debug.Log("✅ ScoreManager este activ (Start)");
    }

    public void AddScore(int points)
    {
        totalScore += points;
        Debug.Log($"🎯 AddScore() apelat — scor total acum = {totalScore}");
    }
}