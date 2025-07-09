using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleSpawner : MonoBehaviour
{
    public GameObject piecePrefab;
    public Transform puzzleGrid;

    IEnumerator Start()
    {
        // UI yerleşimi tamamlanana kadar bekle
        yield return new WaitForEndOfFrame();

        // Seçilen level'e göre yükleme yap
        LoadLevel(LevelData.selectedLevel);
    }

    public void LoadLevel(string name)
    {
        ClearExistingPieces();

        // Sprite'ları Resources klasöründen yükle
        Sprite[] sprites = Resources.LoadAll<Sprite>("Puzzle/" + name);

        if (sprites.Length == 0)
        {
            Debug.LogError("❌ " + name + " yüklenemedi. Sprite dilimlenmiş mi?");
            return;
        }

        List<Sprite> shuffled = new List<Sprite>(sprites);
        Shuffle(shuffled);

        // Puzzle parçalarını oluştur
        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject piece = Instantiate(piecePrefab, puzzleGrid);
            piece.GetComponent<Image>().sprite = shuffled[i];

            PuzzlePiece pp = piece.GetComponent<PuzzlePiece>();
            pp.correctIndex = System.Array.IndexOf(sprites, shuffled[i]);
            pp.currentIndex = i;
        }

        PuzzleManager.Instance.totalPieces = sprites.Length;

        // Grid boyutunu ayarla (taşmaları engellemek için)
        AdjustGridCellSize(sprites.Length);
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void ClearExistingPieces()
    {
        foreach (Transform child in puzzleGrid)
        {
            Destroy(child.gameObject);
        }
    }

    void AdjustGridCellSize(int pieceCount)
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(pieceCount)); // 3x3, 4x4, vs.

        RectTransform gridRect = puzzleGrid.GetComponent<RectTransform>();
        GridLayoutGroup layout = puzzleGrid.GetComponent<GridLayoutGroup>();

        float totalPaddingX = layout.padding.left + layout.padding.right;
        float totalPaddingY = layout.padding.top + layout.padding.bottom;

        float totalSpacingX = layout.spacing.x * (gridSize - 1);
        float totalSpacingY = layout.spacing.y * (gridSize - 1);

        float availableWidth = gridRect.rect.width - totalPaddingX - totalSpacingX;
        float availableHeight = gridRect.rect.height - totalPaddingY - totalSpacingY;

        float cellWidth = availableWidth / gridSize;
        float cellHeight = availableHeight / gridSize;

        layout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
