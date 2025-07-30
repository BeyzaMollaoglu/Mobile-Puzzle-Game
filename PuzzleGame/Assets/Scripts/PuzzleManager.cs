using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [HideInInspector] public int totalPieces;

    [Header("UI References")]
    public GameObject winTextUI;
    public GameObject nextLevelButtonUI;

    [Header("Original Preview")]
    public GameObject originalPreviewPanel;
    public Image originalImage;
    public GameObject winPagePanel;
    public Image previewImageUI;

    [Header("Puzzle Grid")]
    public GridLayoutGroup puzzleGridLayout;  // ← assign this from Inspector

    [Header("Level List")]
    [Tooltip("Resources/Puzzle altındaki klasör adlarını sırayla buraya yaz.")]
    public string[] levelNames = {
        "level1", "level2", "level3", "level4", "level5",
        "level6", "level7", "level8", "level9", "level10",
        "level11", "level12", "level13", "level14", "level15",
        "level16", "level17", "level18", "level19", "level20",
        "level21", "level22", "level23", "level24", "level25",
        "level26", "level27", "level28", "level29", "level30"
    };

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (winTextUI != null) winTextUI.SetActive(false);
        if (nextLevelButtonUI != null) nextLevelButtonUI.SetActive(false);
        if (originalPreviewPanel != null) originalPreviewPanel.SetActive(false);

        if (originalImage != null)
            Debug.Log($"🖼️ OriginalImage.sprite: {(originalImage.sprite != null ? originalImage.sprite.name : "null")}");
    }

    public void CheckWin()
    {
        var pieces = FindObjectsOfType<PuzzlePiece>();
        int correctCount = 0;

        foreach (var p in pieces)
        {
            if (p.transform.GetSiblingIndex() == p.correctIndex)
                correctCount++;
        }

        Debug.Log($"✅ PuzzleManager: doğru={correctCount}, toplam={totalPieces}");

        if (correctCount >= totalPieces)
        {
            StartCoroutine(ShowWinPanelAfterDelay());
        }
    }

    private IEnumerator ShowWinPanelAfterDelay()
    {
        // Küçük bir gecikme vererek önce grid spacing animasyonunu oynatıyoruz
        yield return new WaitForSeconds(0.2f);

        // 🎯 Animate grid spacing collapse (clean visual)
        if (puzzleGridLayout != null)
        {
            Vector2 originalSpacing = puzzleGridLayout.spacing;
            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                puzzleGridLayout.spacing = Vector2.Lerp(originalSpacing, Vector2.zero, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            puzzleGridLayout.spacing = Vector2.zero;
        }

        // Win Page gösterilmeden önce 1 saniye daha bekle
        yield return new WaitForSeconds(1.0f);

        // ─── Yeni eklenecek kilit açma kodu ────────────────────
        // Mevcut level adını ve index'ini bul
        string current = LevelData.selectedLevel;
        int idx = System.Array.IndexOf(levelNames, current);

        // Eğer bir sonraki level varsa ve henüz kilitli ise aç
        if (idx >= 0 && idx < levelNames.Length - 1)
        {
            int currentLevelNumber = idx + 1;  // level1 -> 1, level2 -> 2, vb.
            int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

            if (currentLevelNumber >= unlocked)
            {
                PlayerPrefs.SetInt("UnlockedLevel", currentLevelNumber + 1);
                PlayerPrefs.Save();
            }
        }
        // ─────────────────────────────────────────────────────

        // Win Page’i aktive et
        if (winPagePanel != null)
            winPagePanel.SetActive(true);

        // Orijinal önizlemeyi güncelle (varsa)
        if (previewImageUI != null && originalImage != null)
            previewImageUI.sprite = originalImage.sprite;

        // “Next Level” butonunu aç
        if (nextLevelButtonUI != null)
            nextLevelButtonUI.SetActive(true);

        Debug.Log("🎉 YOU WIN!");
    }
    public void NextLevel()
    {
        string current = LevelData.selectedLevel;
        int idx = System.Array.IndexOf(levelNames, current);

        if (idx >= 0 && idx < levelNames.Length - 1)
        {
            // 1) Mevcut level numarasını bul (level1 için 1, level2 için 2, vb.)
            int currentLevelNumber = idx + 1;

            // 2) Kaçıncı level’e kadar açılmış (default=1)
            int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

            // 3) Eğer bu level, daha önce açılanın eşi ya da üzerindeyse,
            //    PlayerPrefs’e bir sonraki level’i yaz
            if (currentLevelNumber >= unlocked)
            {
                PlayerPrefs.SetInt("UnlockedLevel", currentLevelNumber + 1);
                PlayerPrefs.Save();
            }

            // 4) Sonraki level’e geç
            string next = levelNames[idx + 1];
            LevelData.selectedLevel = next;
            SceneManager.LoadScene("PuzzleScene");
        }
        else
        {
            Debug.Log("🏁 Son level'e ulaşıldı veya level bulunamadı.");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RestartPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleOriginalPreview()
    {
        if (originalPreviewPanel == null || originalImage == null) return;

        bool isActive = originalPreviewPanel.activeSelf;
        originalPreviewPanel.SetActive(!isActive);

        if (!isActive)
        {
            originalImage.gameObject.SetActive(true);
            originalImage.color = Color.white;
            if (originalImage.sprite != null)
                Debug.Log("🖼️ Original preview sprite: " + originalImage.sprite.name);
        }
        else
        {
            originalImage.gameObject.SetActive(false);
        }
    }
}
