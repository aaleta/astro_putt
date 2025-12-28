using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Settings")]
    public Transform spawnPoint;
    public GameObject currentBall;

    public void SpawnBall()
    {
        if (currentBall != null)
        {
            currentBall.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}