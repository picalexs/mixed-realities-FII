using UnityEngine;

public class ConfettiOnScore : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettiParticles;

    private void Awake()
    {
        if (confettiParticles != null) return;
        confettiParticles = GetComponent<ParticleSystem>();
        
        if (confettiParticles == null)
        {
            Debug.LogWarning("No ParticleSystem found on this GameObject. Please assign one or add a ParticleSystem component.");
        }
    }

    private void OnEnable()
    {
        ScoreManager.onScoreChanged += PlayConfetti;
    }

    private void OnDisable()
    {
        ScoreManager.onScoreChanged -= PlayConfetti;
    }

    private void PlayConfetti(int newScore)
    {
        if (confettiParticles != null)
        {
            confettiParticles.Play();
        }
    }
}
