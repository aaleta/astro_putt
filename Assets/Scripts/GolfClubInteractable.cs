using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GolfClubFinal : XRGrabInteractable
{
    [Header("Stabilization")]
    [Tooltip("Higher = Smoother but more 'laggy'. 20-30 is good for golf.")]
    [SerializeField] private float smoothSpeed = 30f;
    [Tooltip("Distance in meters. If hands are closer than this, ignore the second hand.")]
    [SerializeField] private float minGripDistance = 0.10f;

    [Header("Model Configuration")]
    [Tooltip("Which LOCAL axis is the shaft? (Red=Right, Green=Up, Blue=Forward)")]
    [SerializeField] private Vector3 localShaftAxis = Vector3.up; // Change to Vector3.forward if Z-aligned

    // Smoothing Cache
    private Vector3 _smoothedDirection;
    private bool _hasInitializedTwoHand = false;

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Only run on Dynamic (Physics) update
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            base.ProcessInteractable(updatePhase);
            return;
        }

        if (interactorsSelecting.Count >= 2)
        {
            ProcessTwoHandedGrip();
        }
        else
        {
            _hasInitializedTwoHand = false;
            base.ProcessInteractable(updatePhase);
        }
    }

    private void ProcessTwoHandedGrip()
    {
        // Get Interactables
        Transform primaryHand = interactorsSelecting[0].GetAttachTransform(this);
        Transform secondaryHand = interactorsSelecting[1].GetAttachTransform(this);

        // 1. Get Raw Vector between hands
        Vector3 rawDirection = (secondaryHand.position - primaryHand.position).normalized;
        float distance = Vector3.Distance(primaryHand.position, secondaryHand.position);

        // Safety: If hands are too close, abort to avoid math explosions
        if (distance < minGripDistance) return;

        // 2. Smooth the Vector (The "Anti-Shake" Filter)
        // If this is the first frame grabbing, snap immediately. Otherwise, blend.
        if (!_hasInitializedTwoHand)
        {
            _smoothedDirection = rawDirection;
            _hasInitializedTwoHand = true;
        }
        else
        {
            // Slerp moves the vector slowly towards the new target
            _smoothedDirection = Vector3.Slerp(_smoothedDirection, rawDirection, Time.deltaTime * smoothSpeed);
        }

        // 3. Calculate Rotation
        // Start with the Primary Hand's rotation (This keeps your wrist "twist" active for opening the club face)
        Quaternion baseRotation = interactorsSelecting[0].transform.rotation;

        // Calculate where the shaft is CURRENTLY pointing based on just that one hand
        Vector3 currentShaftVector = baseRotation * localShaftAxis;

        // Calculate the rotation needed to bend the shaft from "Current" to "Target"
        Quaternion alignmentRotation = Quaternion.FromToRotation(currentShaftVector, _smoothedDirection);

        // Apply that correction to the base rotation
        Quaternion finalRotation = alignmentRotation * baseRotation;

        // 4. Apply Physics
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.MovePosition(primaryHand.position);
        rb.MoveRotation(finalRotation);
    }

    // VISUAL DEBUGGER: Draw lines in the Scene View to help you setup
    private void OnDrawGizmos()
    {
        if (interactorsSelecting.Count < 1) return;

        Transform hand1 = interactorsSelecting[0].transform;

        // Draw the Axis you selected (GREEN line)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hand1.position, hand1.position + (hand1.rotation * localShaftAxis) * 0.5f);

        if (interactorsSelecting.Count >= 2)
        {
            // Draw the Target Vector (RED line)
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hand1.position, interactorsSelecting[1].transform.position);
        }
    }
}