using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BasketScoreTrigger : MonoBehaviour
{
    public int points = 2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
        {
            Debug.Log("Ball is held — not counting score.");
            return;
        }

        Debug.Log($"Valid basket! +{points} points");
        ScoreManager.Instance?.AddScore(points);
    }
}