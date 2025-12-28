using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Feedback")]
    private Vector3 originalScale;
    public float hoverScale = 1.1f;
    public float speed = 10f;
    public Image buttonImage;

    private Vector3 targetScale;

    void Start()
    {
        originalScale = buttonImage.transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        buttonImage.transform.localScale = Vector3.Lerp(buttonImage.transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered");
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}