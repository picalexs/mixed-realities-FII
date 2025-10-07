using UnityEngine;
using UnityEngine.Events;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    public UnityEvent<GameObject> onEnter;
    public UnityEvent<GameObject> onExit;
    
    private bool isOnLayerMask(GameObject obj)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!isOnLayerMask(other.gameObject)) return;
        onEnter.Invoke(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(!isOnLayerMask(other.gameObject)) return;
        onExit.Invoke(other.gameObject);
    }
}
