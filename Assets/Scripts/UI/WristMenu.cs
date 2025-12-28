using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WristMenu : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform playerHead;
    public NearFarInteractor rightInteractor;
    public float lookThreshold = 0.85f;
    public float animationSpeed = 10f;

    [Header("Data References")]
    public ImageDigitDisplay holesDisplay;
    public ImageDigitDisplay hitsDisplay;
    public string lobbySceneName = "Lobby";

    [Header("Current Status")]
    public bool isOpen = false;
    public bool containsStats = true;

    private float currentScaleY = 0f;

    private void Start()
    {
        if (playerHead == null) playerHead = Camera.main.transform;

        transform.localScale = new Vector3(0.001f, 0, 0.001f);
    }

    private void Update()
    {
        if (playerHead == null) return;

        CheckVisibility();
        AnimateMenu();
    }

    private void CheckVisibility()
    {
        Vector3 dirToHead = (playerHead.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, dirToHead);

        //Debug.Log("Dot: " + dotProduct);

        if (dotProduct > lookThreshold)
        {
            if (!isOpen) OpenMenu();
        }
        else
        {
            if (isOpen) CloseMenu();
        }
    }

    private void OpenMenu()
    {
        isOpen = true;
        rightInteractor.enableFarCasting = true;

        if (containsStats)
        {
            UpdateStats();
        }
    }

    private void CloseMenu()
    {
        rightInteractor.enableFarCasting = false;
        isOpen = false;
    }

    private void AnimateMenu()
    {
        float targetY = isOpen ? 0.001f : 0f;

        currentScaleY = Mathf.Lerp(currentScaleY, targetY, Time.deltaTime * animationSpeed);

        transform.localScale = new Vector3(0.001f, currentScaleY, 0.001f);
    }

    public void UpdateStats()
    {
        if (GameManager.Instance != null)
        {
            holesDisplay.SetValue(GameManager.Instance.currentLevelHoles, false);
            hitsDisplay.SetValue(GameManager.Instance.currentLevelHits, true); 
        }
    }

    public void ReturnToLobby()
    {
        SceneTransition.Instance.GoToScene(lobbySceneName);
    }

    public void ResetScore()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetAllProgress();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}