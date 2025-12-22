using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailClearer : MonoBehaviour
{
    private TrailRenderer trail;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public void ResetTrail()
    {
        if (trail != null)
        {
            trail.Clear();
        }
    }
}