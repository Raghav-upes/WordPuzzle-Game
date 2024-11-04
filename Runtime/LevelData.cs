using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "PuzzleGame/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Words for the Level")]
    public List<string> words; 

    [Header("Correct Words")]
    public List<string> correctWords; 

    [Header("Level Animation")]
    public AnimationClip levelAnimation; 

    [TextArea]
    public string description;
}
