using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshPro tooltipText;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform backgroundPanel;

    private const string TOOLTIP_TEMPLATE = "<color=#00FFFF>{0}</color>\nGRAVITY: <color=#{1}>{2}</color>";
    private bool isShowing = false;
    private float finalScale = 1.0f;

    public void ShowTooltip(string elementName, GravityLevel gravity, Vector3 targetPosition, float sizeFactor, float minScale, float maxScale)
    {
        string colorHex = GetColorForGravity(gravity);
        string gravityText = gravity.ToString().ToUpper();

        tooltipText.text = string.Format(TOOLTIP_TEMPLATE, elementName, colorHex, gravityText);
        PositionTooltip(targetPosition);

        float distanceToUser = Vector3.Distance(Camera.main.transform.position, transform.position);
        finalScale = distanceToUser * sizeFactor;
        finalScale = Mathf.Clamp(finalScale, minScale, maxScale);

        transform.localScale = Vector3.one * finalScale;

        Debug.Log($"Showing tooltip for {elementName} at distance {distanceToUser:F2} with scale {finalScale:F2}");

        if (!isShowing)
        {
            gameObject.SetActive(true);
            if (animator != null)
            {
                animator.Rebind();
                animator.SetBool("isOpen", true);
            }
            isShowing = true;
        }
    }

    public void HideTooltip()
    {
        if (!isShowing) return;

        if (animator != null)
        {
            animator.SetBool("isOpen", false);
        }

        isShowing = false;
    }

    private void Update()
    {
        if (isShowing)
        {
            FaceCamera();
        }
    }

    private void PositionTooltip(Vector3 targetPosition)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 direction = (targetPosition - cameraPos).normalized;

        transform.position = targetPosition - (direction * 0.5f);
    }

    private void FaceCamera()
    {
        if (Camera.main == null) return;

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private string GetColorForGravity(GravityLevel level)
    {
        switch (level)
        {
            case GravityLevel.None:
                return "FFFFFF"; // White
            case GravityLevel.Low:
                return "00FF00"; // Green
            case GravityLevel.Medium:
                return "FFA500"; // Orange
            case GravityLevel.High:
                return "FF0000"; // Red
            default:
                return "FFFFFF";
        }
    }
}