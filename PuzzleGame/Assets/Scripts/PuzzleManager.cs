using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public int totalPieces;
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
        var pieces = UnityEngine.Object.FindObjectsByType<PuzzlePiece>(FindObjectsSortMode.None);

        int correctCount = 0;

        foreach (var piece in pieces)
        {
            if (piece.currentIndex == piece.correctIndex)
                correctCount++;
        }

        Debug.Log($"✅ PuzzleManager: current={pieces.Length}, correct={correctCount}");

        if (correctCount >= pieces.Length)
        {
            winTextUI.gameObject.SetActive(true);
            Debug.Log("🎉 YOU WIN!");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
