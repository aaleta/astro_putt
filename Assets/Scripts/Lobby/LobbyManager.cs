using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [Header("UI References")]
    public Button tutorialButton;
    public Button level1Button;
    public Image level1Image;

    [Header("Scene Names")]
    public string tutorialScene = "Tutorial";
    public string level1Scene = "Course 1";

    void Start()
    {
        bool isTutorialDone = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
        tutorialButton.onClick.AddListener(() => LoadLevel(tutorialScene));

        isTutorialDone = true; // For testing purposes, assume tutorial is done
        if (isTutorialDone)
        {
            level1Button.interactable = true;
            level1Button.onClick.AddListener(() => LoadLevel(level1Scene));
        }
        else
        {
            level1Button.interactable = false;
            Color c = level1Image.color;
            c.a = 0.7f;
            level1Image.color = c;
        }
    }

    void LoadLevel(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}