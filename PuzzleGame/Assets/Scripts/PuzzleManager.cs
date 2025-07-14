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

        // Delay before showing win panel
        yield return new WaitForSeconds(1.0f);

        if (winPagePanel != null) winPagePanel.SetActive(true);

        if (previewImageUI != null && originalImage != null)
            previewImageUI.sprite = originalImage.sprite;

        if (nextLevelButtonUI != null) nextLevelButtonUI.SetActive(true);

        Debug.Log("🎉 YOU WIN!");
    }

    public void NextLevel()
    {
        string current = LevelData.selectedLevel;
        int idx = System.Array.IndexOf(levelNames, current);
        Debug.Log($"{levelNames.Length}");
        Debug.Log($"▶️ Mevcut level: {current}, index: {idx}");

        if (idx >= 0 && idx < levelNames.Length - 1)
        {
            string next = levelNames[idx + 1];
            LevelData.selectedLevel = next;
            Debug.Log($"➡️ Yükleniyor: {next}");
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
