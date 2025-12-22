using UnityEngine;
using UnityEngine.Events; // Needed for UnityEvent

public class HoleAttractor : MonoBehaviour
{
    [Header("Setup")]
    public Transform capturePoint;
    public string targetTag = "Ball";

    [Header("Physics Settings")]
    public float pullForce = 5f;
    public float suctionDamping = 3f;
    public float captureDistance = 0.1f;

    [Header("Events")]
    public UnityEvent onBallCaptured;

    private Rigidbody caughtRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb && !rb.isKinematic)
            {
                caughtRb = rb;

                rb.angularVelocity = Vector3.zero;
                rb.linearDamping = suctionDamping;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag) && caughtRb != null)
        {
            AttractBall(caughtRb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.linearDamping = 0f;
                caughtRb = null;
            }
        }
    }

    private void AttractBall(Rigidbody rb)
    {
        Vector3 directionToHole = (capturePoint.position - rb.position).normalized;

        rb.AddForce(directionToHole * pullForce);
        float distance = Vector3.Distance(rb.position, capturePoint.position);

        if (distance < captureDistance)
        {
            FinalizeCapture(rb.gameObject);
        }
    }

    private void FinalizeCapture(GameObject ball)
    {
        ball.SetActive(false);
        Destroy(ball, 1f);

        onBallCaptured.Invoke();

        this.enabled = false;
    }
}