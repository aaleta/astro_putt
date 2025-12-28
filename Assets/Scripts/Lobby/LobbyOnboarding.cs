using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class LobbyOnboarding : MonoBehaviour
{
    [Header("UI Configuration")]
    public GameObject taskWindow;
    public GameObject levelSelectCanvas;

    [Header("UI Animation & Follow")]
    public float unfoldDuration = 0.5f;
    public Vector3 targetOffset = new(0.3f, -0.1f, 1f);
    public float followSpeed = 5.0f;

    [Header("Checkboxes Configuration")]
    public Image[] taskCheckboxes;
    public float walkTargetDistance = 1f;
    public float minWalkDistance = 0.002f;
    public float maxWalkDistance = 0.5f;
    public float locomotionButtonThreshold = 0.5f;
    public float fadeDuration = 1.0f;

    [Header("Audio")]
    public AudioSource welcomeAssistant;
    public AudioClip introduction;
    public AudioClip realMovement;
    public AudioClip teleport;
    public AudioClip locomotion;
    public AudioClip completion;

    [Header("Input References")]
    public InputActionProperty leftStickMove;
    public InputActionProperty rightStickTeleport;
    public Transform playerHeadset;
    public TeleportationProvider teleportationProvider;

    [Header("Controller Hints")]
    public VRHintController hintController;
    public string teleportAnimName = "Right_Controller_Joystick_U";
    public string moveAnimName = "Right_Controller_Joystick_LR";
    public Vector3 hintPosOffset = new(0.3f, -0.2f, 0.6f);
    public Vector3 hintRotOffset = new(70, -160, 0);

    private int currentStage = -1;
    private Vector3 lastHeadPos;
    private float walkDistance = 0f;
    private bool teleported = false;
    private Vector3 initialWindowScale;

    IEnumerator Start()
    {
        // 1. ROBUST CAMERA FINDER
        if (playerHeadset == null)
        {
            // Try the standard way (requires "MainCamera" tag)
            if (Camera.main != null)
            {
                playerHeadset = Camera.main.transform;
            }
            // Fallback: Find ANY camera (if you forgot the tag)
            else
            {
                var cam = FindFirstObjectByType<Camera>();
                if (cam != null) playerHeadset = cam.transform;
            }
        }

        // 2. SAFETY CHECK
        // If we still didn't find it, log an error and stop to prevent the crash later
        if (playerHeadset == null)
        {
            Debug.LogError("CRITICAL: LobbyOnboarding cannot find the VR Camera! Is it tagged 'MainCamera'?");
            yield break;
        }






        if (GameManager.Instance.HasCompletedTutorial())
        {
            levelSelectCanvas.SetActive(true);
            taskWindow.SetActive(false);
            yield break;
        }
        else
        {
            foreach (var img in taskCheckboxes)
            {
                Color c = img.color;
                c.a = 0;
                img.color = c;
            }

            //levelSelectCanvas.SetActive(false);

            initialWindowScale = taskWindow.transform.localScale;
            taskWindow.transform.localScale = Vector3.zero;
            taskWindow.SetActive(true);

            lastHeadPos = playerHeadset.position;

            if (teleportationProvider != null)
            {
                teleportationProvider.locomotionEnded += OnTeleportComplete;
            }

            // Wait one second before starting
            yield return new WaitForSeconds(1.0f);

            welcomeAssistant.clip = introduction;
            welcomeAssistant.Play();

            // Wait 0.5 to unfold the task window
            yield return new WaitForSeconds(5.5f);
            StartCoroutine(UnfoldWindow());

            yield return new WaitForSeconds(introduction.length - 5f);

            StartStage(0);
        }
    }

    private void OnDestroy()
    {
        if (teleportationProvider != null)
        {
            teleportationProvider.locomotionEnded -= OnTeleportComplete;
        }
    }

    private void OnTeleportComplete(LocomotionProvider system)
    {
        if (currentStage == 1)
        {
            teleported = true;
        }
    }

    private void Update()
    {
        if (playerHeadset == null) return;

        if (currentStage == 0) // Real Walk
        {
            float dist = Vector3.Distance(playerHeadset.position, lastHeadPos);
            if (dist > minWalkDistance && dist < maxWalkDistance)
            {
                walkDistance += dist;
                lastHeadPos = playerHeadset.position;
            }
            else if (dist >= maxWalkDistance) // Probably the player teleported
            {
                lastHeadPos = playerHeadset.position;
            }

            //Debug.Log($"Walked distance: {walkDistance:F2} / {walkTargetDistance:F2}");

            if (walkDistance >= walkTargetDistance)
            {
                CompleteStage(0);
            }
        }
        else if (currentStage == 1) // Teleport
        { 
            if (teleported)
            {
                CompleteStage(1);
            }
        }
        else if (currentStage == 2) // Locomotion
        {
            Vector2 stickInput = leftStickMove.action.ReadValue<Vector2>();
            float moveAmount = stickInput.magnitude;

            float distMoved = Vector3.Distance(playerHeadset.position, lastHeadPos);
            lastHeadPos = playerHeadset.position;

            if (moveAmount > locomotionButtonThreshold && distMoved > minWalkDistance)
            {
                CompleteStage(2);
            }
        }
    }

    private void LateUpdate()
    {
        if (playerHeadset == null) return;

        if (taskWindow != null && taskWindow.activeSelf && currentStage != -2)
        {
            //Debug.Log("The player headset position is: " + playerHeadset.position);
            Vector3 targetPosition = playerHeadset.TransformPoint(targetOffset);

            //Debug.Log("The target position for the task window is: " + targetPosition);

            taskWindow.transform.position = Vector3.Lerp(taskWindow.transform.position, targetPosition, Time.deltaTime * followSpeed);
            Quaternion targetRotation = Quaternion.LookRotation(taskWindow.transform.position - playerHeadset.position);

            taskWindow.transform.rotation = Quaternion.Slerp(taskWindow.transform.rotation, targetRotation, Time.deltaTime * followSpeed);
        }
    }

    IEnumerator UnfoldWindow()
    {
        float timer = 0f;
        while (timer < unfoldDuration)
        {
            timer += Time.deltaTime;
            float t = timer / unfoldDuration;

            taskWindow.transform.localScale = Vector3.Lerp(Vector3.zero, initialWindowScale, t);
            yield return null;
        }
        taskWindow.transform.localScale = initialWindowScale;
    }

    void StartStage(int stageIndex)
    {
        currentStage = stageIndex;
        teleported = false;
        lastHeadPos = playerHeadset.position;

        switch (stageIndex)
        {
            case 0: 
                PlayClip(realMovement); 
                break;
            case 1: 
                PlayClip(teleport);
                if (hintController != null)
                    hintController.ShowHint(VRHintController.Hand.Right, teleportAnimName, hintPosOffset, hintRotOffset);
                break;
            case 2: 
                PlayClip(locomotion);
                if (hintController != null)
                    hintController.ShowHint(VRHintController.Hand.Left, moveAnimName, hintPosOffset, hintRotOffset);
                break;
        }
    }

    void CompleteStage(int stageIndex)
    {
        currentStage = -2;

        if (hintController != null) hintController.HideHint();

        Debug.Log($"Completed stage {stageIndex}");

        if (stageIndex < taskCheckboxes.Length)
        {
            StartCoroutine(FadeInImage(taskCheckboxes[stageIndex]));
        }

        StartCoroutine(WaitAndNext(stageIndex));
    }

    IEnumerator FadeInImage(Image targetImage)
    {
        float timer = 0f;
        Color c = targetImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            targetImage.color = c;
            yield return null;
        }

        c.a = 1f;
        targetImage.color = c;
    }

    IEnumerator WaitAndNext(int stageIndex)
    {
        yield return new WaitForSeconds(1f);
        if (stageIndex < 2)
        {
            StartStage(stageIndex + 1);
        }
        else
        {
            FinishTutorial();
        }
    }

    void FinishTutorial()
    {
        PlayClip(completion);

        StartCoroutine(FadeInImage(taskCheckboxes[^1]));

        levelSelectCanvas.SetActive(true);

        GameManager.Instance.CompleteTutorial();
    }

    void PlayClip(AudioClip clip)
    {
        welcomeAssistant.Stop();
        welcomeAssistant.clip = clip;
        welcomeAssistant.Play();
    }

    public bool ShouldShowHint()
    {
        return currentStage == 1 || currentStage == 2;
    }
}
