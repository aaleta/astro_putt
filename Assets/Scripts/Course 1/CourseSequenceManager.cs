using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CourseSequenceManager : MonoBehaviour
{
    [System.Serializable]
    public class HoleStage
    {
        public string name = "Hole 1";
        public GameObject holeVisuals;
    }

    [Header("Configuration")]
    public GameObject ballPrefab;
    public float delayBetweenHoles = 1.5f;

    [Header("The Course")]
    public List<HoleStage> stages;

    private int currentIndex = 0;

    void Start()
    {
        InitializeCourse();
    }

    void InitializeCourse()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if (stages[i].holeVisuals != null)
            {
                stages[i].holeVisuals.SetActive(i == 0);
            }
        }
    }

    public void OnHoleFinished()
    {
        SwitchToNextHole();
    }

    private void SwitchToNextHole()
    {
        if (currentIndex < stages.Count)
        {
            if (stages[currentIndex].holeVisuals != null)
                stages[currentIndex].holeVisuals.SetActive(false);
        }

        currentIndex++;

        if (currentIndex < stages.Count)
        {
            HoleStage nextStage = stages[currentIndex];

            if (nextStage.holeVisuals != null)
                nextStage.holeVisuals.SetActive(true);
        }
        else
        {
            Debug.Log("Course Completed!");
        }
    }
}