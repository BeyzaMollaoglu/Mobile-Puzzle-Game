using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public int totalPieces;
    public int correctPieces;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckWin()
    {
        if (correctPieces >= totalPieces)
        {
            Debug.Log("🎉 Puzzle Tamamlandı!");
            // Buraya oyun kazanma ekranı veya sahne geçişi ekleyebilirsin
        }
    }
}
