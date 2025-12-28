using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class EndGameMenu : MonoBehaviour
{
    [Header("UI References")]
    public Button lobbyButton;
    public Button level1Button;

    [Header("Settings")]
    public float unfoldDuration = 0.5f;
    public Vector3 targetOffset = new(0.3f, -0.1f, 1f);
    public float followSpeed = 5.0f;
    public Vector3 initialWindowScale = new(0.5f, 0.6f, 0);

    [Header("Input References")]
    public Transform playerHeadset;
    public NearFarInteractor[] playerInteractors;

    [Header("Scene Names")]
    public string lobbyScene = "Lobby";
    public string course1Scene = "Course 1";


    private void Start()
    {
        this.transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        Debug.Log("EndGameMenu Enabled");
        StartCoroutine(UnfoldWindow());

        if (lobbyButton != null)
        {
            lobbyButton.onClick.AddListener(() => SceneTransition.Instance.GoToScene(lobbyScene));
        }

        if (level1Button != null)
        {
            level1Button.onClick.AddListener(() => SceneTransition.Instance.GoToScene(course1Scene));
        }

        ToggleFarCasting(true);
    }

    private void LateUpdate()
    {
        if (this.isActiveAndEnabled)
        {
            Vector3 targetPosition = playerHeadset.TransformPoint(targetOffset);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * followSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(this.transform.position - playerHeadset.position);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * followSpeed);
        }
    }

    IEnumerator UnfoldWindow()
    {
        float timer = 0f;
        while (timer < unfoldDuration)
        {
            timer += Time.deltaTime;
            float t = timer / unfoldDuration;

            this.transform.localScale = Vector3.Lerp(Vector3.zero, initialWindowScale, t);
            yield return null;
        }
        this.transform.localScale = initialWindowScale;
    }

    private void ToggleFarCasting(bool state)
    {
        if (playerInteractors != null)
        {
            foreach (var interactor in playerInteractors)
            {
                if (interactor != null)
                {
                    interactor.enableFarCasting = state;
                }
            }
        }
    }
}