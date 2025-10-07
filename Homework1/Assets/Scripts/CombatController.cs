using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour {
    private ProximityDetector _proximityDetector;
    
    private void Awake()
    {
        _proximityDetector = GetComponentInChildren<ProximityDetector>();
        if (_proximityDetector == null)
        {
            Debug.LogError($"CombatController on {name}: ProximityDetector not found in children!", this);
        }
        if (GetComponentInParent<Rigidbody>() == null)
        {
            Debug.LogError($"CombatController on {name}: Rigidbody not found in parent!", this);
        }
    }
    
    private void OnEnable()
    {
        if (_proximityDetector == null) return;
        _proximityDetector.onEnter.AddListener(OnEnterCombat);
        _proximityDetector.onExit.AddListener(OnExitCombat);
    }

    private void OnDisable()
    {
        if (_proximityDetector == null) return;
        _proximityDetector.onEnter.RemoveListener(OnEnterCombat);
        _proximityDetector.onExit.RemoveListener(OnExitCombat);
    }

    private void OnEnterCombat(GameObject attacker)
    {
        Debug.Log($"Entering Combat with {attacker.name}");
    }
    
    private void OnExitCombat(GameObject attacker)
    {
        Debug.Log($"Exiting Combat with {attacker.name}");
    }
}