using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ThrowTracker : MonoBehaviour
{
    [SerializeField]
    private Vector3 lastReleasePosition;
    public Vector3 getLastReleasePosition => lastReleasePosition;

    private XRGrabInteractable _grab;

    private void Awake()
    {
        _grab = GetComponent<XRGrabInteractable>();
        _grab.selectExited.AddListener(_ => SaveReleasePosition());
    }

    private void SaveReleasePosition()
    {
        lastReleasePosition = transform.position;
        Debug.Log($"Ball was released from position {lastReleasePosition}");
    }

    public bool IsHeld()
    {
        return _grab != null && _grab.isSelected;
    }
}