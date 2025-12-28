using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public bool isTutorialCompleted = false;

    public List<LevelScore> levelScores = new();
}

[Serializable]
public struct LevelScore
{
    public string levelName;
    public int holes;
    public int shots;
}