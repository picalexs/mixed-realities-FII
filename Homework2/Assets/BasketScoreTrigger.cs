using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BasketScoreTrigger : MonoBehaviour
{
    public int points = 2;

    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        // verificăm dacă mingea e ținută de cineva
        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
        {
            Debug.Log(" Mingea e ținută — nu contăm scorul.");
            return;
        }

        // dacă mingea e liberă, dăm puncte
        Debug.Log($"Coș valid! +{points} puncte");
        ScoreManager.Instance?.AddScore(points);
    }
}