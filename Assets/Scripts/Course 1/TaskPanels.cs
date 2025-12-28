using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanels : MonoBehaviour
{
    public static TaskPanels Instance;
    private static readonly List<TaskPanels> allPanels = new();

    private static int sharedCompletedCount = 0;
    private static bool[] tasksCompleted;

    [Header("Checkboxes Configuration")]
    public Image[] taskCheckboxes;
    public float fadeDuration = 1.0f;

    [Header("End Game Configuration")]
    public GameObject endGameMenu;
    private readonly string holeName = "Course1";
    private void Awake()
    {
        Instance = this;
        allPanels.Add(this);
    }

    private void OnDestroy()
    {
        allPanels.Remove(this);
    }

    private void Start()
    {
        if (tasksCompleted == null || tasksCompleted.Length != taskCheckboxes.Length)
        {
            tasksCompleted = new bool[taskCheckboxes.Length];
            sharedCompletedCount = 0;
        }

        if (endGameMenu != null) endGameMenu.SetActive(false);

        for (int i = 0; i < taskCheckboxes.Length; i++)
        {
            Color c = taskCheckboxes[i].color;
            c.a = tasksCompleted[i] ? 1f : 0f;
            taskCheckboxes[i].color = c;
        }
    }

    public void CompleteTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < tasksCompleted.Length)
        {
            if (!tasksCompleted[taskIndex])
            {
                tasksCompleted[taskIndex] = true;
                sharedCompletedCount++;

                foreach (var panel in allPanels)
                {
                    panel.AnimateTaskCompletion(taskIndex);
                }

                if (sharedCompletedCount >= tasksCompleted.Length)
                {
                    foreach (var panel in allPanels)
                    {
                        panel.ShowEndGameMenu();
                    }
                }
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterScore(holeName);
            GameManager.Instance.SaveGame();

            Debug.Log($"Score Registered for {holeName}: Holes - {GameManager.Instance.currentLevelHoles}, Hits - {GameManager.Instance.currentLevelHits}");
        }
    }

    public void AnimateTaskCompletion(int taskIndex)
    {
        if (taskIndex < taskCheckboxes.Length)
        {
            StartCoroutine(FadeInImage(taskCheckboxes[taskIndex]));
        }
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

    public void ShowEndGameMenu()
    {
        if (endGameMenu != null)
        {
            endGameMenu.SetActive(true);
        }
    }
}