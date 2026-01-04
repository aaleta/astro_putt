using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [Header("References")]
    public RocketController rocket;
    public Transform capturePoint;

    [Header("Game Logic")]
    public int holeIndex = 0;

    [Header("Forces")]
    public float pullForce = 0.1f;
    public float liftForce = 0.1f;
    public float absorptionDistance = 0.2f;

    private bool isCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb && !rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                AttractBall(rb);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb && !rb.isKinematic)
            {
                AttractBall(rb);
            }
        }
    }

    private void AttractBall(Rigidbody rb)
    {
        Vector3 centerPos = new(transform.position.x, rb.position.y, transform.position.z);
        Vector3 directionToCenter = (centerPos - rb.position).normalized;
        Vector3 liftDirection = transform.up;

        rb.linearDamping = 2f;

        rb.AddForce(directionToCenter * pullForce);
        rb.AddForce(liftDirection * liftForce);

        float distanceToRocket = Vector3.Distance(rb.position, capturePoint.position);

        if (distanceToRocket < absorptionDistance)
        {
            CaptureBall(rb.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().linearDamping = 0f;
        }
    }

    private void CaptureBall(GameObject ball)
    {
        ball.SetActive(false);
        Destroy(ball, 1f);

        if (TaskPanel.Instance != null && !isCompleted)
        {
            isCompleted = true;
            GameManager.Instance.AddHole();
            TaskPanel.Instance.CompleteTask(holeIndex);
        }

        rocket.IgniteAndLaunch();

        this.enabled = false;
    }
}