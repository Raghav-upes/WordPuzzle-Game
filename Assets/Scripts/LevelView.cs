using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class LevelView : MonoBehaviour
{

    public Transform wordButtonsContainer;
    public Button wordButtonPrefab;
    public Animator animator; 

    public TMP_Text formedSentenceText;
    public TMP_Text LevelNumber;
    public Button clearSentenceButton;
    public Button lastWordClear;
    public Button checkButton;
    public List<Button> wordButtons = new List<Button>();

    public GameObject FeedbackFrame;
    public Color correctAnswer;
    public Color incorrectAnswer;

    public float FeedbackHideTime = 2f;

    private void OnEnable()
    {

        var controller = FindObjectOfType<LevelController>();
        controller.OnSentenceUpdated += UpdateFormedSentence;
        controller.OnLevelComplete += StopAnimation;
        controller.OnEnterAnswer += MessageType;

        clearSentenceButton.onClick.AddListener(controller.ClearSentence);
        lastWordClear.onClick.AddListener(controller.ClearLastWord);
        checkButton.onClick.AddListener(controller.CheckSolution);
    }

    private void OnDisable()
    {

        var controller = FindObjectOfType<LevelController>();
        controller.OnSentenceUpdated -= UpdateFormedSentence;
        controller.OnLevelComplete -= StopAnimation;
        controller.OnEnterAnswer -= MessageType;

        clearSentenceButton.onClick.RemoveListener(controller.ClearSentence);
        lastWordClear.onClick.RemoveListener(controller.ClearLastWord);
        checkButton.onClick.RemoveListener(controller.CheckSolution);
    }

    public void Initialize(LevelData levelData)
    {

        var controller = FindObjectOfType<LevelController>();
        LevelNumber.text = "Level " + (controller.currentLevelIndex + 1).ToString();
       
        
        foreach (Button button in wordButtons)
        {
            Destroy(button.gameObject);
        }
        wordButtons.Clear();

        foreach (string word in levelData.words)
        {
            Button wordButton = Instantiate(wordButtonPrefab, wordButtonsContainer);
            wordButton.GetComponentInChildren<TMP_Text>().text = word;
            wordButtons.Add(wordButton);
        }

        foreach (var btn in FindObjectsOfType<Button>())
        {
            btn.interactable = true;
        }

        formedSentenceText.text = "";








        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, levelData.levelAnimation));
        aoc.ApplyOverrides(anims);
        animator.runtimeAnimatorController = aoc;

        animator.enabled = true;
        animator.Play("DefaultAnimationClip", -1, 0f);
    }

    private void UpdateFormedSentence(string sentence)
    {
        formedSentenceText.text = sentence;
    }

    private void MessageType(bool type) { 
       
        if(type)
        {
            FeedbackFrame.GetComponentInChildren<TMP_Text>().text = "It is correct answer";
            FeedbackFrame.GetComponentInChildren<TMP_Text>().color = correctAnswer;
            FeedbackFrame.GetComponent<Image>().color = correctAnswer;

        }
        else
        {
            FeedbackFrame.GetComponentInChildren<TMP_Text>().text = "It is incorrect answer";
            FeedbackFrame.GetComponentInChildren<TMP_Text>().color = incorrectAnswer;
            FeedbackFrame.GetComponent<Image>().color = incorrectAnswer;

        }
        StartCoroutine(showAndHideFeedback());
    }


   private IEnumerator showAndHideFeedback()
    {
        FeedbackFrame.gameObject.SetActive(true);
        yield return new WaitForSeconds(FeedbackHideTime);
        FeedbackFrame.gameObject.SetActive(false);
    }

    private void StopAnimation()
    {
        foreach (var btn in FindObjectsOfType<Button>())
        {
            btn.interactable = false;
        }
        if (animator.runtimeAnimatorController != null)
        {
            animator.Play("DefaultAnimationClip", -1, 0f);
            animator.enabled = false;
          
        }
    }
}
