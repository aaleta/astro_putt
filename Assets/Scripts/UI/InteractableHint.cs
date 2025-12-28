using UnityEngine;

public class InteractableGaze : MonoBehaviour
{
    [Header("Hint Configuration")]
    public VRHintController hintController;
    public string animationName = "Right_Controller_Press_Bumper";
    public Vector3 hintOffset = new(0.0f, 0.0f, 0.6f);
    public Vector3 rotationOffset = new(0, 110, 0);

    [Header("Raycast Settings")]
    public float gazeDistance = 10f;
    //public LayerMask gazeLayerMask;


    private Transform cameraTransform;
    private bool isHovered = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        CheckGaze();
    }

    private void CheckGaze()
    {
        Ray ray = new(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, gazeDistance))//, gazeLayerMask))
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
        Debug.Log("Gaze entered on " + this.name);
        if (!isHovered)
        {
            isHovered = true;
            hintController.ShowHint(VRHintController.Hand.Right, animationName, hintOffset, rotationOffset);
        }
    }

    private void OnGazeExit()
    {
        if (isHovered)
        {
            isHovered = false;
            hintController.HideHint();
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