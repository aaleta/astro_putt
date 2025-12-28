using UnityEngine;

public class LobbyScoreboard : MonoBehaviour
{
    [Header("Tutorial References")]
    public string level1ID = "Tutorial";
    public ImageDigitDisplay level1HolesDisplay;
    public ImageDigitDisplay level1ShotsDisplay;

    [Header("Course 1 References")]
    public string level2ID = "Course1";
    public ImageDigitDisplay level2HolesDisplay;
    public ImageDigitDisplay level2ShotsDisplay;

    [Header("Total References")]
    public ImageDigitDisplay totalShotsDisplay;

    private void Start()
    {
        // Ensure GameManager exists
        if (GameManager.Instance == null) return;

        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        int l1Holes = GameManager.Instance.GetBestHoles(level1ID);
        int l1Shots = GameManager.Instance.GetBestScore(level1ID);

        int l2Holes = GameManager.Instance.GetBestHoles(level2ID);
        int l2Shots = GameManager.Instance.GetBestScore(level2ID);

        level1HolesDisplay.SetValue(l1Holes, false);
        level1ShotsDisplay.SetValue(l1Shots, true);

        Debug.Log($"Lobby Scoreboard - {level1ID}: Holes = {l1Holes}, Shots = {l1Shots}");

        level2HolesDisplay.SetValue(l2Holes, false);
        level2ShotsDisplay.SetValue(l2Shots, true);

        Debug.Log($"Lobby Scoreboard - {level2ID}: Holes = {l2Holes}, Shots = {l2Shots}");

        int grandTotal = l1Shots + l2Shots;
        totalShotsDisplay.SetValue(grandTotal, true);
    }
}