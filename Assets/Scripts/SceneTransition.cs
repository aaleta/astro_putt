using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Components")]
    public Canvas fadeCanvas;
    public Image fadeImage;

    [Header("Settings")]
    public float fadeDuration = 1.0f;
    public Color fadeColor = Color.black;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
            fadeImage.raycastTarget = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera newCamera = Camera.main;
        if (newCamera != null && fadeCanvas != null)
        {
            fadeCanvas.worldCamera = newCamera;
            fadeCanvas.planeDistance = 0.5f;
        }

        StartCoroutine(Fade(1f, 0f));
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(object sceneIdentifier)
    {
        // 1. Block inputs
        fadeImage.raycastTarget = true;

        // 2. Fade Out
        yield return StartCoroutine(Fade(0f, 1f));

        // 3. Load Scene Asynchronously
        AsyncOperation operation;
        if (sceneIdentifier is int index)
            operation = SceneManager.LoadSceneAsync(index);
        else
            operation = SceneManager.LoadSceneAsync((string)sceneIdentifier);

        operation.allowSceneActivation = false;

        // Wait until it's loaded
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 4. Activate the scene
        operation.allowSceneActivation = true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, endAlpha);

        if (endAlpha == 0f) fadeImage.raycastTarget = false;
    }
}