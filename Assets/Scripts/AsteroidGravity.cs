using UnityEngine;

public class AsteroidGravity : MonoBehaviour
{
    [Header("Settings")]
    public float gravityStrength = 100f;

    [Tooltip("How much drag to apply when touching the surface")]
    public float surfaceDrag = 2f;
    public float surfaceAngularDrag = 2f;

    [Header("Inner Core")]
    [Tooltip("Drag the SOLID collider of the asteroid here")]
    public Collider asteroidSurface;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb && !rb.isKinematic)
        {
            ApplyGravity(rb);
        }
    }

    private void ApplyGravity(Rigidbody rb)
    {
        Vector3 difference = transform.position - rb.position;
        float distance = difference.magnitude;
        Vector3 direction = difference.normalized;

        // 2. NEWTONIAN FALLOFF (Realism)
        // Force gets stronger as distance gets smaller.
        float gravityFalloff = gravityStrength / Mathf.Pow(Mathf.Max(distance, 0.1f), 2);

        // 3. Apply the Pull
        rb.AddForce(direction * gravityFalloff);

        // 4. Align the ball's "Down" to the planet (Optional visual polish)
        // This helps the ball roll naturally rather than spinning wildly
        // rb.rotation = Quaternion.FromToRotation(transform.up, -direction) * rb.rotation;
    }

    // --- SURFACE HANDLING (THE STICKING LOGIC) ---

    // We need to know if the ball is touching the SOLID rock, not just the gravity field.
    // Since this script is on the Field, we listen for collision events on the Core via the ball.
    // Actually, it's easier to detect if the ball is *very* close to the center.

    void FixedUpdate()
    {
        // Pure Trigger logic handles the pull. 
        // For collision friction, we rely on the Physics Material of the asteroid, 
        // OR we can hack it here if the ball enters a "Surface Zone".
    }
}