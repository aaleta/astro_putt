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
            currentBall.transform.position = spawnPoint.position;
            currentBall.transform.rotation = Quaternion.identity;

            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}