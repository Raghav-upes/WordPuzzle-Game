using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    private LevelData currentLevelData;
    private LevelView levelView;


    public event Action<string> OnSentenceUpdated;
    public event Action OnLevelComplete;
    public event Action<bool> OnEnterAnswer;

    public List<LevelData> levels; 
    private List<string> selectedWords = new List<string>();

    public int currentLevelIndex = 0;

    private void Start()
    {
        levelView = FindObjectOfType<LevelView>();  
   SetUpLevels();
     
        
    }


    public void SetUpLevels()
    {
        currentLevelData = GetCurrentLevelData();
        levelView.Initialize(currentLevelData);
        AssignButtonCallbacks();
        ClearSentence();
    }

    private void AssignButtonCallbacks()
    {
        foreach (Button button in levelView.wordButtons)
        {
            button.onClick.AddListener(() => OnWordSelected(button.GetComponentInChildren<TMP_Text>().text));
        }
    }

    private void OnWordSelected(string selectedWord)
    {
        selectedWords.Add(selectedWord);
        UpdateFormedSentenceDisplay();
      /*  CheckSolution();*/
    }

    private void UpdateFormedSentenceDisplay()
    {
        string formedSentence = string.Join(" ", selectedWords);
       formedSentence=FirstCharToUpper(formedSentence);
        OnSentenceUpdated?.Invoke(formedSentence);
    }

    public void CheckSolution()
    {
        bool isCorrect = true;

        for (int i = 0; i < selectedWords.Count; i++)
        {
            if (i >= currentLevelData.correctWords.Count || selectedWords[i] != currentLevelData.correctWords[i])
            {
      
                isCorrect = false;
               
                break;
            }
        }

        if (isCorrect && selectedWords.Count == currentLevelData.correctWords.Count)
        {
            OnLevelComplete?.Invoke();
            OnEnterAnswer?.Invoke(true);
            Debug.Log("Correct! Level Solved.");

            Invoke("LoadNextLevel", 2f);
        }
        else
        OnEnterAnswer?.Invoke(false);
    }


    public void ClearLastWord()
    {
        if (selectedWords.Count > 0)
        {

            selectedWords.RemoveAt(selectedWords.Count - 1);
        }

      
        string updatedSentence = string.Join(" ", selectedWords);


        OnSentenceUpdated?.Invoke(updatedSentence);
        Debug.Log("Last word removed. Updated sentence: " + updatedSentence);
    }


    public void ClearSentence()
    {
        selectedWords.Clear();
        OnSentenceUpdated?.Invoke("");
        Debug.Log("Sentence cleared.");
    }


    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            currentLevelIndex = 0;
        }

        SetUpLevels();
    }

    public static string FirstCharToUpper(string input)
    {
        if (String.IsNullOrEmpty(input))
            throw new ArgumentException("ARGH!");
        return input.First().ToString().ToUpper() + input.Substring(1);
    }



    public LevelData GetCurrentLevelData()
    {
        if (currentLevelIndex < levels.Count)
        {
            return levels[currentLevelIndex];
        }
        return null; 
    }
}
