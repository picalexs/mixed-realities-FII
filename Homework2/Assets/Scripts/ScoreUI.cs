using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        ScoreManager.onScoreChanged += UpdateScoreDisplay;
    }

    private void OnDisable()
    {
        ScoreManager.onScoreChanged -= UpdateScoreDisplay;
    }

    private void Start()
    {
        UpdateScoreDisplay(ScoreManager.instance != null ? ScoreManager.instance.GetCurrentScore() : 0);
    }

    private void UpdateScoreDisplay(int newScore)
    {
        if (!scoreText) return;
        scoreText.text = $"Score: {newScore}";
    }
}

