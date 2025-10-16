using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ThrowTracker : MonoBehaviour
{
    public Vector3 lastReleasePos;  
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(_ => SaveReleasePoint());
    }

    private void SaveReleasePoint()
    {
        lastReleasePos = transform.position;
        Debug.Log($"Mingea a fost eliberată de la poziția {lastReleasePos}");
    }

    public bool IsHeld()
    {
        return grab != null && grab.isSelected;
    }
}