using UnityEngine;

public class BasketScoreTrigger : MonoBehaviour
{
    public int points = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log($"🏀 Coș! +{points} puncte");
            ScoreManager.Instance?.AddScore(points);
        }
    }
}