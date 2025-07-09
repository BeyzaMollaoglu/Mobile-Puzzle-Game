using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public int totalPieces;
    public int correctPieces;

    public GameObject winTextUI;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void CheckWin()
    {
        var pieces = Object.FindObjectsByType<PuzzlePiece>(FindObjectsSortMode.None);
        int correctCount = 0;

        foreach (var piece in pieces)
        {
            Debug.Log($"[Kontrol] {piece.name} → correct: {piece.correctIndex}, current: {piece.currentIndex}");

            if (!piece.IsInCorrectPosition())
            {
                Debug.Log("🚫 Parça yanlışta.");
                return;
            }
            else
            {
                correctCount++;
            }
        }

        Debug.Log($"✅ Tüm parçalar doğru yerde! ({correctCount}/{pieces.Length})");
        if (winTextUI != null)
            winTextUI.SetActive(true);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
