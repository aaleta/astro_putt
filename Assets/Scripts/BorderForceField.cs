/*
 * Outer walls of the game area that become visible when the player approaches them.
 */

using UnityEngine;

public class BorderForceField : MonoBehaviour
{
    [Header("Visuals")]
    public MeshRenderer wallRenderer;
    public float fadeSpeed = 5f;

    [Header("Audio")]
    public AudioSource buzzAudio;

    [Header("Settings")]
    public Transform player;
    public float triggerRadius = 6.0f;
    private Transform centerPoint;

    [Header("Debug Info")]
    [SerializeField] private float currentDistance;

    private Material wallMat;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;

    void Start()
    {
        if (wallRenderer != null)
        {
            wallMat = wallRenderer.material;
            UpdateAlpha(0f);
        }

        if (centerPoint == null) centerPoint = this.transform;
    }

    void Update()
    {
        if (player == null || centerPoint == null) return;

        Vector3 playerFlat = new(player.position.x, 0, player.position.z);
        Vector3 centerFlat = new(centerPoint.position.x, 0, centerPoint.position.z);
        currentDistance = Vector3.Distance(playerFlat, centerFlat);

        if (currentDistance > triggerRadius)
        {
            targetAlpha = 1f;
            if (buzzAudio != null && !buzzAudio.isPlaying) buzzAudio.Play();
        }
        else
        {
            targetAlpha = 0f;
            if (buzzAudio != null) buzzAudio.Stop();
        }

        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
        UpdateAlpha(currentAlpha);
    }
    void UpdateAlpha(float alphaVal)
    {
        if (wallMat != null)
        {
            Color newColor = wallMat.color;
            newColor.a = alphaVal;
            wallMat.color = newColor;
        }
    }
}
