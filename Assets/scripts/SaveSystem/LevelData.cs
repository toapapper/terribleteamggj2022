using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<Level> data;
}

[System.Serializable]
public class Level
{
    public int id;
    public int score;
}
