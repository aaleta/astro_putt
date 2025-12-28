using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GolfClubHaptics : XRGrabInteractable
{
    [Header("Haptics")]
    [Tooltip("How fast (m/s) must you swing to get max vibration?")]
    public float maxHitVelocity = 5f;
    public float vibrationDuration = 0.1f;

    private float lastHapticTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Time.time < lastHapticTime + vibrationDuration) return;
            lastHapticTime = Time.time;

            float impactSpeed = collision.relativeVelocity.magnitude;
            float intensity = Mathf.Clamp01(impactSpeed / maxHitVelocity);
            intensity = Mathf.Max(intensity, 0.1f);

            foreach (var interactor in interactorsSelecting)
            {
                Debug.Log($"Triggering haptic on interactor {interactor.transform.gameObject.name} with intensity {intensity}");
                TriggerHaptic(interactor, intensity);
            }
        }
    }

    private void TriggerHaptic(IXRSelectInteractor interactor, float intensity)
    {
        if (interactor is XRBaseInputInteractor inputInteractor)
        {
            inputInteractor.SendHapticImpulse(intensity, vibrationDuration);
        }
    }
}
