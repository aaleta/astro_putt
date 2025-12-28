using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SaveData currentSaveData;
    public int currentLevelHoles = 0;
    public int currentLevelHits = 0;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("GameManager instance created.");

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Loading and Saving Game Data
    private void Initialize()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "astro_putt_save.json");
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(currentSaveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved to: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            currentSaveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            currentSaveData = new SaveData();
        }

        Debug.Log("Game Loaded from: " + saveFilePath);
        Debug.Log($"Tutorial Completed: {currentSaveData.isTutorialCompleted}");
        Debug.Log($"Levels Recorded: {currentSaveData.levelScores.Count}");
        Debug.Log("Level Scores:");
        foreach (var score in currentSaveData.levelScores)
        {
            Debug.Log($"Level: {score.levelName}, Holes: {score.holes}, Shots: {score.shots}");
        }
    }

    public void ResetAllProgress()
    {
        Debug.Log("Resetting all game progress...");

        ResetLevelStats();

        currentSaveData = new SaveData();
        SaveGame();

        Debug.Log("Global Game Reset: Save file overwritten and stats cleared.");
    }

    // Writing Game Data
    public void AddHole()
    {
        currentLevelHoles++;
        currentLevelHoles = Mathf.Min(currentLevelHoles, 3);
        Debug.Log("Hole completed. Total holes this level: " + currentLevelHoles);
    }

    public void AddHit()
    {
        currentLevelHits++;
        Debug.Log("Hit recorded. Total hits this level: " + currentLevelHits);
    }

    public void ResetLevelStats()
    {
        currentLevelHoles = 0;
        currentLevelHits = 0;
    }

    public void CompleteTutorial()
    {
        currentSaveData.isTutorialCompleted = true;
        SaveGame();
    }

    public void RegisterScore(string levelName)
    {
        LevelScore existingScore = currentSaveData.levelScores.Find(x => x.levelName == levelName);

        if (existingScore.levelName == null)
        {
            LevelScore newScore = new() { levelName = levelName, holes = currentLevelHoles, shots = currentLevelHits };
            currentSaveData.levelScores.Add(newScore);
            SaveGame();

            Debug.Log($"New Record for {levelName}: {currentLevelHoles} holes & {currentLevelHits} shots");
        }
        else if (currentLevelHoles > existingScore.holes)
        {
            currentSaveData.levelScores.RemoveAll(x => x.levelName == levelName);
            LevelScore updatedScore = new() { levelName = levelName, holes = currentLevelHoles, shots = currentLevelHits };
            currentSaveData.levelScores.Add(updatedScore);
            SaveGame();

            Debug.Log($"Improved Record for {levelName}: {currentLevelHoles} holes & {currentLevelHits} shots");
        }
        else if (currentLevelHoles == existingScore.holes && currentLevelHits < existingScore.shots)
        {
            currentSaveData.levelScores.RemoveAll(x => x.levelName == levelName);
            LevelScore updatedScore = new() { levelName = levelName, holes = currentLevelHoles, shots = currentLevelHits };
            currentSaveData.levelScores.Add(updatedScore);
            SaveGame();

            Debug.Log($"Improved Record for {levelName}: {currentLevelHoles} holes & {currentLevelHits} shots");
        }
    }

    // Retrieving Game Data

    public int GetBestHoles(string levelName)
    {
        LevelScore score = currentSaveData.levelScores.Find(x => x.levelName == levelName);
        return score.levelName != null ? score.holes : 0;
    }

    public int GetBestScore(string levelName)
    {
        LevelScore score = currentSaveData.levelScores.Find(x => x.levelName == levelName);
        return score.levelName != null ? score.shots : 0;
    }

    public bool HasCompletedTutorial()
    {
        return currentSaveData.isTutorialCompleted;
    }

    // Debugging Utility

    [ContextMenu("Delete Save File")]
    public void DeleteSaveFile()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "astro_putt_save.json");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");

            currentSaveData = new SaveData();
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }
    }
}