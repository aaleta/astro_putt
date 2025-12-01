using UnityEngine;
using UnityEngine.Events;

public class SpawnerButton : MonoBehaviour
{
    [Header("Configuration")]
    public float pressThreshold = 0.04f;
    public float deadTime = 1f;

    [Header("Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private Vector3 startPos;
    private bool isPressed = false;
    private float lastPressTime;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float distance = startPos.y - transform.localPosition.y;

        if (!isPressed && distance >= pressThreshold)
        {
            if (Time.time - lastPressTime > deadTime)
            {
                Pressed();
            }
        }
        else if (isPressed && distance < pressThreshold * 0.5f)
        {
            Released();
        }
    }

    void Pressed()
    {
        isPressed = true;
        lastPressTime = Time.time;

        onPressed.Invoke();
    }

    void Released()
    {
        isPressed = false;
        onReleased.Invoke();
    }
}