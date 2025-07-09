using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSpawner : MonoBehaviour
{
    public GameObject piecePrefab;
    public Transform puzzleGrid;

    void Start()
    {
        LoadLevel(LevelData.selectedLevel);
    }

    public void LoadLevel(string name)
    {
        ClearExistingPieces();

        // Sprite’ları isim sonundaki sayıya göre sırala
        Sprite[] sprites = Resources.LoadAll<Sprite>("Puzzle/" + name)
            .OrderBy(s =>
            {
                Match match = Regex.Match(s.name, @"_(\d+)$");
                return match.Success ? int.Parse(match.Groups[1].Value) : 0;
            })
            .ToArray();

        if (sprites.Length == 0)
        {
            Debug.LogError("❌ Sprite yok veya yanlış klasör: Puzzle/" + name);
            return;
        }

        // Shuffle edilmiş sprite'ları tut
        List<Sprite> shuffled = new List<Sprite>(sprites);
        Shuffle(shuffled);

        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject piece = Instantiate(piecePrefab, puzzleGrid);
            Image img = piece.GetComponent<Image>();
            img.sprite = shuffled[i];

            PuzzlePiece pp = piece.GetComponent<PuzzlePiece>();

            string spriteName = shuffled[i].name;

            // 🎯 Doğru sprite'ın orijinal listedeki index'ini bul
            int correctIndex = System.Array.FindIndex(sprites, s => s.name == spriteName);

            // ✅ Güvenlik: bulunamadıysa logla
            if (correctIndex == -1)
            {
                Debug.LogError($"❗ Sprite adı eşleşmedi: {spriteName}");
            }

            // 📌 Atamalar
            pp.correctIndex = correctIndex;
            pp.currentIndex = i;

            Debug.Log($"🧩 {spriteName} - correct: {correctIndex}, current: {i}");
        }

        PuzzleManager.Instance.totalPieces = sprites.Length;
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
