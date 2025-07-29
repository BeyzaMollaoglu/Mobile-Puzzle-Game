using UnityEngine;
using UnityEngine.UI;

public class StorySelector : MonoBehaviour
{
    [Header("UI References")]
    public Button[] storyButtons;      // 1, 2, 3. story ikonları
    [Range(0,1)] public float lockedAlpha = 0.5f;  // kilitliyi ne kadar soluk gösterelim

    const int levelsPerStory = 10;   // her hikayede 10 level

    void Start()
    {
        UpdateStories();
    }

    void UpdateStories()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < storyButtons.Length; i++)
        {
            // hikaye i, kapsadığı level numaraları: (i*10+1) … (i*10+10)
            int firstLevelOfThisStory = i * levelsPerStory + 1;
            bool isUnlocked = unlockedLevel >= firstLevelOfThisStory;

            // tıklanabilirlik
            storyButtons[i].interactable = isUnlocked;

            // solukluk (butonun Image component’inden)
            var img = storyButtons[i].GetComponent<Image>();
            Color c = img.color;
            c.a = isUnlocked ? 1f : lockedAlpha;
            img.color = c;

            // eğer child olarak ayrı bir “LockIcon” varsa:
            // var lockIcon = storyButtons[i].transform.Find("LockIcon");
            // if (lockIcon != null) lockIcon.gameObject.SetActive(!isUnlocked);
        }
    }
}
