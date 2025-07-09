using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [HideInInspector] public int totalPieces;
    public GameObject winTextUI; // inspector’dan birbirine bağla
    public GameObject nextLevelButtonUI; // inspector
    [Tooltip("Level adlarını sırayla buraya yazın (Resources/Puzzle klasöründeki alt klasör adları)")]
    public string[] levelNames = {"level1", "level2", "level3"}; 
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
            // child‐index = parça şu anda hangi hücrede?
            if (p.transform.GetSiblingIndex() == p.correctIndex)
                correctCount++;
        }

        Debug.Log($"✅ PuzzleManager: doğru={correctCount}, toplam={totalPieces}");

        if (correctCount >= totalPieces)
        {
            if (winTextUI != null){
                winTextUI.SetActive(true);
            }
                
        
            nextLevelButtonUI.SetActive(true);
            Debug.Log("🎉 YOU WIN!");
        }
    }
    public void NextLevel()
    {
        string current = LevelData.selectedLevel;
        int idx = System.Array.IndexOf(levelNames, current);

        // Eğer bulundu ve bir sonraki varsa
        if (idx != -1 && idx < levelNames.Length - 1)
        {
            LevelData.selectedLevel = levelNames[idx + 1];
            // Aynı sahneyi yeniden yükle (PuzzleSpawner Start’da yeni level’i okuyacak)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("⚠️ Son leveldesiniz ya da levelNames dizisi hatalı.");
            // İstersen MainScene’e dön:
            // SceneManager.LoadScene("MainScene");
        }
    }
    // İstersen ana menü butonuna bağla
    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
