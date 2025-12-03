using UnityEngine;

public class SafetyDoor : MonoBehaviour
{
    [Header("Configuration")]
    public Transform doorVisuals;
    public Transform openPoint;
    public Transform closedPoint;
    public Transform player;
    public float xTrigger = -3f;

    [Header("Settings")]
    public float closeSpeed = 25f;
    public float openSpeed = 2f;

    private Vector3 targetPosition;
    private float currentSpeed;
    private bool isClosed = false;

    void Start()
    {
        targetPosition = openPoint.position;
        doorVisuals.position = openPoint.position;
        currentSpeed = openSpeed;
    }

    void Update()
    {
        if (player.transform.position.x > xTrigger && !isClosed)
        {
            ActivateDoor(true);
        } 
        else if (player.transform.position.x <= xTrigger && isClosed)
        {
            ActivateDoor(false);
        }    

        doorVisuals.position = Vector3.MoveTowards(doorVisuals.position, targetPosition, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(doorVisuals.position, closedPoint.position) < 0.01f)
        {
            if (!isClosed)
            {
                isClosed = true;
            }
        }
    }

    public void ActivateDoor(bool shouldClose)
    {
        if (shouldClose)
        {
            targetPosition = closedPoint.position;
            currentSpeed = closeSpeed;
        }
        else
        {
            targetPosition = openPoint.position;
            currentSpeed = openSpeed;
            isClosed = false;
        }
    }
}