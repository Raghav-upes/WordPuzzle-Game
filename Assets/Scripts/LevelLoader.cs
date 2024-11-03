using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public LevelData currentLevelData;

    void Start()
    {
        if (currentLevelData != null)
        {
            Debug.Log("Words: " + string.Join(", ", currentLevelData.words));
            Debug.Log("Correct Words: " + string.Join(", ", currentLevelData.correctWords));
            Debug.Log("Animation: " + currentLevelData.levelAnimation);
            Debug.Log("Description: " + currentLevelData.description);
        }
        else
        {
            Debug.LogWarning("No LevelData assigned!");
        }
    }
}
