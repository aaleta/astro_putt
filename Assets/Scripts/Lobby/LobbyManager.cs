using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Debug.Log("LobbyManager Start");


        bool isTutorialDone = GameManager.Instance.HasCompletedTutorial();
        tutorialButton.onClick.AddListener(() => SceneTransition.Instance.GoToScene(tutorialScene));

        //isTutorialDone = true;
        if (isTutorialDone)
        {
            level1Button.interactable = true;
            level1Button.onClick.AddListener(() => SceneTransition.Instance.GoToScene(level1Scene));
        }
        else
        {
            level1Button.interactable = false;
            Color c = level1Image.color;
            c.a = 0.7f;
            level1Image.color = c;
        }
    }

    public void RegisterLobbyButtons(Button playBtn, Button tutorialBtn)
    {
        tutorialButton = playBtn;
        level1Button = tutorialBtn;
    }
}