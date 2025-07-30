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

            // 1) Metni ayarla ve kilit durumuna göre göster/gizle
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
            txt.text = levelNumber.ToString();
            bool isUnlocked = levelNumber <= unlockedLevel;
            txt.enabled = isUnlocked;
            btn.interactable = isUnlocked;

            // 2) LockIcon child’ını aç/kapa, raycast’i kapat
            var lockTrans = btn.transform.Find("LockIcon");
            if (lockTrans != null)
            {
                lockTrans.gameObject.SetActive(!isUnlocked);
                var lockImg = lockTrans.GetComponent<Image>();
                lockImg.raycastTarget = false;
            }

            // 3) ButonScaler component’ini al
            var scaler = btn.GetComponent<ButtonScaler>();

            // 4) Animasyon bittikten sonra çağrılacakları temizle ve ekle
            scaler.onAnimationComplete.RemoveAllListeners();
            if (isUnlocked)
            {
                int lvl = levelNumber;  // closure için kopya
                scaler.onAnimationComplete.AddListener(() =>
                {
                    FindObjectOfType<LevelMenu>().LoadLevelByNumber(lvl);
                });
            }

            // 5) (Opsiyonel) Arkayı da soluklaştır
            var bg = btn.GetComponent<Image>();
            Color c = bg.color;
            c.a = isUnlocked ? 1f : 0.5f;
            bg.color = c;
        }
    }

    public void GoBackToStories()
    {
        levelPanel.SetActive(false);
        storyPanel.SetActive(true);
    }
}
