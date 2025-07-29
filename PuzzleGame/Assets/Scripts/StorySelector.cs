using UnityEngine;
using UnityEngine.UI;

public class StorySelector : MonoBehaviour
{
    public Button[] storyButtons;
    public float lockedAlpha = 0.5f;  // isteğe bağlı, eğer istersen arkayı az da olsa soluk tutarsın

    const int levelsPerStory = 10;

    void Start()
    {
        UpdateStories();
    }

    void UpdateStories()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < storyButtons.Length; i++)
        {
            var btn = storyButtons[i];
            int firstLvl = i * levelsPerStory + 1;
            bool isUnlocked = unlockedLevel >= firstLvl;

            btn.interactable = isUnlocked;

            // 1) LockIcon'u aç/kapa
            var lockIcon = btn.transform.Find("LockIcon");
            if (lockIcon != null)
                lockIcon.gameObject.SetActive(!isUnlocked);

            // 2) Butonun altındaki tüm Image komponentlerini bul
            var images = btn.GetComponentsInChildren<Image>(true);

            foreach (var img in images)
            {
                // kilit ikonu değilse (LockIcon adını kullanıyoruz)
                if (img.gameObject.name == "LockIcon")  
                    continue;

                // img: bu ya arka planın ya da içerikteki hayvan resminin Image'ı
                var c = img.color;
                c.a = isUnlocked ? 1f : lockedAlpha;
                img.color = c;
            }
        }
    }
}
