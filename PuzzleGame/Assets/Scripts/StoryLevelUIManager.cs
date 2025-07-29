using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryLevelUIManager : MonoBehaviour
{
    public GameObject storyPanel;
    public GameObject levelPanel;
    public TMP_Text storyTitleText;
    public Button[] levelButtons; 

    private int currentStoryIndex;

    public void OpenStory(int storyIndex)
    {
        currentStoryIndex = storyIndex;

        storyPanel.SetActive(false);
        levelPanel.SetActive(true);

        int levelsPerStory = levelButtons.Length;
        int baseLevel = storyIndex * levelsPerStory + 1;
        storyTitleText.text = $"Story {storyIndex + 1}";

        // Kaçıncı level’e kadar açılmış?
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = baseLevel + i;
            Button btn = levelButtons[i];
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
            txt.text = levelNumber.ToString();

            // kilit açık mı?
            bool isUnlocked = levelNumber <= unlockedLevel;
            btn.interactable = isUnlocked;
            txt.color        = isUnlocked
                               ? Color.white 
                               : Color.gray;

            // önce tüm listener’ları temizle
            btn.onClick.RemoveAllListeners();
            if (isUnlocked)
            {
                // local capture için yeni değişken
                int lvl = levelNumber;
                btn.onClick.AddListener(() =>
                {
                    FindObjectOfType<LevelMenu>()
                        .LoadLevelByNumber(lvl);
                });
            }
        }
    }

    public void GoBackToStories()
    {
        levelPanel.SetActive(false);
        storyPanel.SetActive(true);
    }
}
