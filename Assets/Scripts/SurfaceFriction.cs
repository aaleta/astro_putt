using UnityEngine;

public class SurfaceFriction : MonoBehaviour
{
    [Header("Friction Settings")]
    public float surfaceDrag = 3f;
    public float surfaceAngularDrag = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearDamping = surfaceDrag;
            rb.angularDamping = surfaceAngularDrag;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }
    }
}