using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryLevelUIManager : MonoBehaviour
{
    public GameObject storyPanel;              // İçinde Story 1, 2, 3 butonları var
    public GameObject levelPanel;              // İçinde 10 level butonu var
    public TMP_Text storyTitleText;            // Üstte “Story 1” gibi yazar
    public Button[] levelButtons;              // 10 buton

    private int currentStoryIndex;

    public void OpenStory(int storyIndex)
    {
        currentStoryIndex = storyIndex;

        storyPanel.SetActive(false);
        levelPanel.SetActive(true);

        int baseLevel = storyIndex * 10 + 1;
        storyTitleText.text = $"Story {storyIndex + 1}";

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = baseLevel + i;
            levelButtons[i].GetComponentInChildren<TMP_Text>().text = levelNumber.ToString();

            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() =>
            {
                FindObjectOfType<LevelMenu>().LoadLevelByNumber(levelNumber);
            });
        }
    }

    public void GoBackToStories()
    {
        levelPanel.SetActive(false);
        storyPanel.SetActive(true);
    }
}
