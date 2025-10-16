using UnityEngine;

public class BasketScoreTrigger : MonoBehaviour
{
    public int basePoints = 2;             // puncte de bază
    public Transform hoopCenter;           // centrul coșului (Empty pe inel)
    public float distanceMultiplier = 0.5f; // cât bonus primești pe metru

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        var tracker = other.GetComponent<ThrowTracker>();
        if (tracker == null)
        {
            Debug.Log("Mingea nu are ThrowTracker.");
            return;
        }

        if (tracker.IsHeld())
        {
            Debug.Log("Mingea e ținută – nu contăm coșul.");
            return;
        }

        float distance = Vector3.Distance(tracker.lastReleasePos, hoopCenter.position);
        int score = basePoints + Mathf.RoundToInt(distance * distanceMultiplier);

        Debug.Log($"Coș valid! +{score} puncte (distanță: {distance:F2} m)");
        ScoreManager.Instance?.AddScore(score);
    }
}