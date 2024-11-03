using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevelData; 
    private List<bool> wordSelection; 
    private bool isPreviewActive = false;

    private Vector2 scrollPosition;

    [MenuItem("PuzzleGame/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Configuration", EditorStyles.boldLabel);


        currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevelData, typeof(LevelData), false);

        if (currentLevelData != null)
        {
           
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 50)); // Adjust height to leave space for buttons

            DrawLevelConfiguration();
            DrawPreviewControls();

            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a LevelData object to edit.", MessageType.Warning);
        }
    }

    private void DrawLevelConfiguration()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Words", EditorStyles.boldLabel);


        for (int i = 0; i < currentLevelData.words.Count; i++)
        {
            currentLevelData.words[i] = EditorGUILayout.TextField("Word " + (i + 1), currentLevelData.words[i]);
        }

        if (GUILayout.Button("Add Word"))
        {
            currentLevelData.words.Add("");
        }

        if (GUILayout.Button("Remove Word"))
        {
            if (currentLevelData.words.Count > 0)
                currentLevelData.words.RemoveAt(currentLevelData.words.Count - 1);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Correct Words", EditorStyles.boldLabel);


        for (int i = 0; i < currentLevelData.correctWords.Count; i++)
        {
            currentLevelData.correctWords[i] = EditorGUILayout.TextField("Correct Word " + (i + 1), currentLevelData.correctWords[i]);
        }

        if (GUILayout.Button("Add Correct Word"))
        {
            currentLevelData.correctWords.Add("");
        }

        if (GUILayout.Button("Remove Correct Word"))
        {
            if (currentLevelData.correctWords.Count > 0)
                currentLevelData.correctWords.RemoveAt(currentLevelData.correctWords.Count - 1);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
        currentLevelData.levelAnimation = (AnimationClip)EditorGUILayout.ObjectField("Level Animation", currentLevelData.levelAnimation, typeof(AnimationClip), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
        currentLevelData.description = EditorGUILayout.TextArea(currentLevelData.description, GUILayout.Height(60));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentLevelData);
        }
    }

   


    private void DrawPreviewControls()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Preview", EditorStyles.boldLabel);

        if (isPreviewActive)
        {
            if (wordSelection == null || wordSelection.Count != currentLevelData.words.Count)
            {
                wordSelection = new List<bool>(new bool[currentLevelData.words.Count]);
            }

            for (int i = 0; i < currentLevelData.words.Count; i++)
            {
                wordSelection[i] = EditorGUILayout.Toggle(currentLevelData.words[i], wordSelection[i]);
            }

            if (GUILayout.Button("Check Solution"))
            {
                CheckSolution();
            }

            if (GUILayout.Button("Exit Preview"))
            {
                isPreviewActive = false;
                ResetWordSelection();
            }
        }
        else
        {
            if (GUILayout.Button("Start Preview"))
            {
                isPreviewActive = true;
                ResetWordSelection();
            }
        }
    }

    private void CheckSolution()
    {
        bool isCorrect = true;
        foreach (string correctWord in currentLevelData.correctWords)
        {
            int index = currentLevelData.words.IndexOf(correctWord);
            if (index < 0 || !wordSelection[index])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct! Triggering animation...");
            if (currentLevelData.levelAnimation != null)
            {
                PlayAnimationPreview(currentLevelData.levelAnimation);
            }
        }
        else
        {
            Debug.LogWarning("Incorrect selection. Try again.");
        }
    }

    private void PlayAnimationPreview(AnimationClip animationClip)
    {
        Debug.Log($"Animation '{animationClip.name}' would play here.");
    }

    private void ResetWordSelection()
    {
        wordSelection = new List<bool>(new bool[currentLevelData.words.Count]);
    }
}
