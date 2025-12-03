using UnityEngine;

public class AsteroidGravity : MonoBehaviour
{
    [Header("Settings")]
    public float gravityStrength = 100f;

    [Tooltip("How much drag to apply when touching the surface")]
    public float surfaceDrag = 5f;
    public float surfaceAngularDrag = 5f;

    private float defaultDrag = 0f;
    private float defaultAngularDrag = 0.05f;

    [Header("Inner Core")]
    [Tooltip("Drag the SOLID collider of the asteroid here")]
    public Collider asteroidSurface;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            defaultDrag = other.attachedRigidbody.linearDamping;
            defaultAngularDrag = other.attachedRigidbody.angularDamping;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            other.attachedRigidbody.linearDamping = defaultDrag;
            other.attachedRigidbody.angularDamping = defaultAngularDrag;
        }
    }

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
        // The force of particle 1 on particle 2 is:
        // F21 = - G * (m1*m2) u21 / r^2
        // Where:
        // F21 = Force on object 2 due to object 1
        // G = Gravitational constant (we use gravityStrength here)
        // m1 = Mass of object 1 (we ignore this for simplicity)
        // m2 = Mass of object 2
        // u21 = Unit vector from object 1 to object 2, u21 = (r2 - r1) / |r2 - r1|
        // r = Distance between object 1 and object 2

        // Particle 1 is always the asteroid (this object)
        // We can use u12 = -u21 to simplify the direction calculation.
        // Thus we calculate the difference as (position1 - position2)

        // 1. Calculate Distance and Direction
        Vector3 difference = transform.position - rb.position;
        float distance = difference.magnitude;
        Vector3 direction = difference.normalized;

        // Apply surface drag if touching the asteroid surface
        Vector3 closestPointOnSurface = asteroidSurface.ClosestPoint(rb.position);
        float distanceToSurface = Vector3.Distance(rb.position, closestPointOnSurface);

        if (distanceToSurface < 0.1f)
        {
            rb.linearDamping = surfaceDrag;
            rb.angularDamping = surfaceAngularDrag;
        }
        else
        {
            rb.linearDamping = defaultDrag;
            rb.angularDamping = defaultAngularDrag;
        }

        // 2. Newtonian Gravity strength
        float gravity = gravityStrength * rb.mass / Mathf.Pow(Mathf.Max(distance, 0.01f), 2);

        // 3. Apply the Pull
        rb.AddForce(direction * gravity);
    }
}