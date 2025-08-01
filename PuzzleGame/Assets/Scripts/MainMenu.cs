using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Animation")]
    public Animator playButtonAnimator;

    [Header("Continue Button UI")]
    public TMP_Text playButtonText;

    const int levelsPerStory = 10; 
    int unlockedLevel; 
    int storyIndex, levelInStory;       

    void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        storyIndex   = (unlockedLevel - 1) / levelsPerStory;          
        levelInStory = ((unlockedLevel - 1) % levelsPerStory) + 1;     

        playButtonText.text = $"Continue: Story {storyIndex + 1} - Level {levelInStory}";
    }

    public void StartGame()
    {
        playButtonAnimator.SetTrigger("Pressed");

        LevelData.selectedLevel = $"Level{unlockedLevel}";

        StartCoroutine(LoadSceneDelayed(0.3f));
    }

    IEnumerator LoadSceneDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("PuzzleScene");
    }

    public void LevelButton()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
