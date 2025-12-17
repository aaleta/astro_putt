using UnityEngine;

public class VRHintController : MonoBehaviour
{
    public enum Hand { Left, Right }


    [Header("Follow Settings")]
    public Transform playerHeadset;
    public float followSpeed = 8f;

    [Header("Controller Models")]
    public GameObject leftControllerModel;
    public GameObject rightControllerModel;

    [Header("Internal References")]
    private Animator leftAnimator;
    private Animator rightAnimator;

    private Vector3 currentOffset;
    private Vector3 currentRotationOffset;
    private bool isActive = false;

    void Awake()
    {
        if (playerHeadset == null) playerHeadset = Camera.main.transform;

        if (leftControllerModel != null) leftAnimator = leftControllerModel.GetComponent<Animator>();
        if (rightControllerModel != null) rightAnimator = rightControllerModel.GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public void ShowHint(Hand hand, string animStateName, Vector3 positionOffset, Vector3 rotationOffset)
    {
        isActive = true;
        currentOffset = positionOffset;
        currentRotationOffset = rotationOffset;

        Debug.Log($"Showing hint on {hand} hand with animation {animStateName}");

        gameObject.SetActive(true);

        if (hand == Hand.Left)
        {
            if (rightControllerModel) rightControllerModel.SetActive(false);
            if (leftControllerModel)
            {
                leftControllerModel.SetActive(true);
                if (leftAnimator) leftAnimator.Play(animStateName, 0, 0f);
            }
        }
        else if (hand == Hand.Right)
        {
            if (leftControllerModel) leftControllerModel.SetActive(false);
            if (rightControllerModel)
            {
                rightControllerModel.SetActive(true);
                if (rightAnimator) rightAnimator.Play(animStateName, 0, 0f);
            }
        }

        transform.SetPositionAndRotation(playerHeadset.TransformPoint(currentOffset), playerHeadset.rotation * Quaternion.Euler(currentRotationOffset));
    }

    public void HideHint()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (isActive && playerHeadset != null)
        {
            Vector3 targetPos = playerHeadset.TransformPoint(currentOffset);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

            Quaternion targetRot = playerHeadset.rotation * Quaternion.Euler(currentRotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * followSpeed);
        }
    }
}