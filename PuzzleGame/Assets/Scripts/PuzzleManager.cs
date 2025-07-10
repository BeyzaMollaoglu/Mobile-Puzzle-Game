using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [HideInInspector] public int totalPieces;

    [Header("UI References")]
    public GameObject winTextUI;            // Inspector’dan atanır
    public GameObject nextLevelButtonUI;    // Inspector’dan atanır

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
        if (winTextUI != null)
            winTextUI.SetActive(false);
        if (nextLevelButtonUI != null)
            nextLevelButtonUI.SetActive(false);
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
            if (winTextUI != null)
                winTextUI.SetActive(true);

            if (nextLevelButtonUI != null)
                nextLevelButtonUI.SetActive(true);

            Debug.Log("🎉 YOU WIN!");
        }
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
            // İstersen buradan ana menüye dönebilirsin:
            // SceneManager.LoadScene("MainScene");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
