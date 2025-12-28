using UnityEngine;

public enum GravityLevel { None, Low, Medium, High }

public class AsteroidGaze : MonoBehaviour
{
    [Header("Data")]
    public string celestialName = "Asteroid";
    public GravityLevel gravity = GravityLevel.None;

    [Header("References")]
    public TooltipController tooltipController;
    public Transform tooltipSpawnPoint;

    [Header("Raycast Settings")]
    public float gazeDistance = 10f;
    public LayerMask gazeLayerMask;

    [Header("Tooltip Settings")]
    [Tooltip("How big the tooltip looks relative to distance.")]
    public float sizeFactor = 0.15f;
    [Tooltip("The smallest it can ever be")]
    public float minScale = 0.2f;
    [Tooltip("The largest it can ever be")]
    public float maxScale = 0.5f;

    private Transform cameraTransform;
    private bool isHovered = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        if (tooltipController != null)
        {
            tooltipController.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckGaze();
    }

    private void CheckGaze()
    {
        Ray ray = new(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, gazeDistance, gazeLayerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            //Debug.Log($"Raycast hit: {hit.transform.name}");

            if (hit.transform == this.transform)
            {
                OnGazeEnter(hit.point);
                return;
            }
        }

        OnGazeExit();
    }

    private void OnGazeEnter(Vector3 hitPoint)
    {
        //Debug.Log($"Gazing at {celestialName}");
        if (!isHovered)
        {
            isHovered = true;
            if (tooltipSpawnPoint != null)
            {
                hitPoint = tooltipSpawnPoint.position;
            }
            tooltipController.ShowTooltip(celestialName, gravity, hitPoint, sizeFactor, minScale, maxScale);
        }        
    }

    private void OnGazeExit()
    {
        if (isHovered)
        {
            isHovered = false;
            tooltipController.HideTooltip();
        }
    }

    private void OnDrawGizmos()
    {
        if (Camera.main != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * gazeDistance);
        }
    }
}