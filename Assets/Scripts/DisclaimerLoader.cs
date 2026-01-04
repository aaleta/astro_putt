using UnityEngine;
using UnityEngine.SceneManagement;

public class DisclaimerLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}