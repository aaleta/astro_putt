using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonVRFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Feedback")]
    private Vector3 originalScale;
    public float hoverScale = 1.1f;
    public float speed = 10f;

    [Header("Hint Configuration")]
    public VRHintController hintController;
    public string animationName = "Right_Controller_Press_Trigger";
    public Vector3 hintOffset = new(0.0f, 0.0f, 0.6f);
    public Vector3 rotationOffset = new(0, 110, 0);

    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;

        // Prevent showing hints during onboarding
        if (LobbyOnboarding.Instance != null && LobbyOnboarding.Instance.ShouldShowHint())
        {
            return;
        }

        if (hintController != null)
        {
            hintController.ShowHint(VRHintController.Hand.Right, animationName, hintOffset, rotationOffset);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;

        // Prevent showing hints during onboarding
        if (LobbyOnboarding.Instance != null && LobbyOnboarding.Instance.ShouldShowHint())
        {
            return;
        }

        if (hintController != null)
        {
            hintController.HideHint();
        }
    }
}