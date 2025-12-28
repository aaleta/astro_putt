using UnityEngine;

public class BallHitCounter : MonoBehaviour
{
    [Header("Configuration")]
    public string clubTag = "Club";
    public float hitCooldown = 0.5f;

    private float lastHitTime = 0f;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetLevelStats();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ball collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag(clubTag))
        {
            Debug.Log("Ball hit detected by club.");
            if (Time.time - lastHitTime > hitCooldown)
            {
                lastHitTime = Time.time;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddHit();

                    Debug.Log("Ball hit by club. Total hits this level: " + GameManager.Instance.currentLevelHits);
                } else
                {
                    Debug.LogWarning("GameManager instance not found. Cannot record hit.");
                }
            }
        }
    }
}