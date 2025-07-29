// StoryLevelUIManager.cs 
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryLevelUIManager : MonoBehaviour
{
    public GameObject storyPanel; 
    public GameObject levelPanel;  
    public TMP_Text storyTitleText;  
    public Button[] levelButtons;    // 10 buton  
    const int levelsPerStory = 10;

    private int currentStoryIndex;

    public void OpenStory(int storyIndex)
    {
        currentStoryIndex = storyIndex;
        storyPanel.SetActive(false);
        levelPanel.SetActive(true);

        int baseLevel = storyIndex * levelsPerStory + 1;
        storyTitleText.text = $"Story {storyIndex + 1}";

        // Kaçıncı level’e kadar açıldı?
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = baseLevel + i;
            Button btn = levelButtons[i];

            // 1) Metni ayarla
            var txt = btn.GetComponentInChildren<TMP_Text>();
            txt.text = levelNumber.ToString();

            // 2) Kilit durumu
            bool isUnlocked = levelNumber <= unlockedLevel;
            btn.interactable = isUnlocked;

            // 3) Olayları ayarla...
            btn.onClick.RemoveAllListeners();
            if (isUnlocked) { /* listener ekle */ }

            // 4) Kilit ikonunu aç/kapa
            var lockIcon = btn.transform.Find("LockIcon");
            if (lockIcon != null)
                lockIcon.gameObject.SetActive(!isUnlocked);

            // 5) Kilitliyse sayıyı devre dışı bırak
            txt.enabled = isUnlocked;
        }
    }

    public void GoBackToStories()
    {
        levelPanel.SetActive(false);
        storyPanel.SetActive(true);
    }
}
