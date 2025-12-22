using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportManager : MonoBehaviour
{
    [Header("References")]
    public Transform playerRoot;
    public Transform mainCamera;
    public Transform teleportTarget;
    public ParticleSystem teleportEffect;

    [Header("Timing")]
    public float delayBeforeMove = 3.0f;
    public float fadeDuration = 1.0f;

    [Header("Fade Settings")]
    public Canvas fadeCanvas;
    public Image fadeImage;

    public void StartTeleportSequence()
    {
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        // 1. Activate the teleport effect
        if (teleportEffect != null)
        {
            teleportEffect.transform.position = playerRoot.position;
            teleportEffect.transform.SetParent(playerRoot);

            teleportEffect.gameObject.SetActive(true);
            teleportEffect.Play();
        }

        // 2. Wait a bit before moving
        yield return new WaitForSeconds(delayBeforeMove - fadeDuration);

        // 3. Fade out to black
        yield return StartCoroutine(Fade(0f, 1f));

        // 4. Teleport the player
        if (teleportEffect != null)
        {
            //teleportEffect.transform.SetParent(null);
            teleportEffect.Stop();
        }

        playerRoot.position = teleportTarget.position;
        playerRoot.rotation = teleportTarget.rotation;

        // 5. Fade in from black
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new(color.r, color.g, color.b, endAlpha);
    }
}