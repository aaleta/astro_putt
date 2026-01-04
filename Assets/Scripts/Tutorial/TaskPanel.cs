using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    public static TaskPanel Instance;

    [Header("Checkboxes Configuration")]
    public Image[] taskCheckboxes;
    public float fadeDuration = 1.0f;

    [Header("End Game Configuration")]
    public GameObject endGameMenu;
    
    private int completedCount = 0;
    private bool[] holeCompleted = new bool[3] { false, false, false };
    private readonly string holeName = "Tutorial";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (endGameMenu != null) endGameMenu.SetActive(false);

        foreach (var img in taskCheckboxes)
        {
            Color c = img.color;
            c.a = 0;
            img.color = c;
        }

        //Debug.Log("Show end game menu for testing purposes.");
        //ShowEndGameMenu();
    }

    public void CompleteTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < taskCheckboxes.Length && !holeCompleted[taskIndex])
        {
            holeCompleted[taskIndex] = true;
            completedCount++;
            StartCoroutine(FadeInImage(taskCheckboxes[taskIndex]));
        }

        if (completedCount >= taskCheckboxes.Length)
        {
            ShowEndGameMenu();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterScore(holeName);
            GameManager.Instance.SaveGame();

            Debug.Log($"Score Registered for {holeName}: Holes - {GameManager.Instance.currentLevelHoles}, Hits - {GameManager.Instance.currentLevelHits}");
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

    private void ShowEndGameMenu()
    {
        if (endGameMenu != null)
        {
            Debug.Log("All tasks completed! Showing end game menu.");
            endGameMenu.SetActive(true);
        }
    }
}