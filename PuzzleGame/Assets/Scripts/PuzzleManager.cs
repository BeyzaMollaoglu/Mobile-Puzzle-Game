using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [HideInInspector] public int totalPieces;
    public GameObject winTextUI; // inspector’dan birbirine bağla

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (winTextUI != null)
            winTextUI.SetActive(false);
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
            if (winTextUI != null)
                winTextUI.SetActive(true);

            Debug.Log("🎉 YOU WIN!");
        }
    }

    // İstersen ana menü butonuna bağla
    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
