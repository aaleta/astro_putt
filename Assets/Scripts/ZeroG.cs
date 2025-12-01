using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ZeroGObject : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
    }

    void OnEnable()
    {
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
    }
}