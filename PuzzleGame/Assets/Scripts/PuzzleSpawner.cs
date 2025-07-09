using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSpawner : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject piecePrefab;
    public Transform puzzleGrid; // GridLayoutGroup'lu Panel

    void Start()
    {
        if (string.IsNullOrEmpty(LevelData.selectedLevel))
        {
            Debug.LogError("❌ LevelData.selectedLevel boş!");
            return;
        }
        LoadLevel(LevelData.selectedLevel);
    }

    public void LoadLevel(string levelName)
    {
        // 1) Temizle
        foreach (Transform c in puzzleGrid) Destroy(c.gameObject);

        // 2) Kaynaktan sırala
        Sprite[] sprites = Resources
            .LoadAll<Sprite>("Puzzle/" + levelName)
            .OrderBy(s =>
            {
                var m = Regex.Match(s.name, @"_(\d+)$");
                return m.Success ? int.Parse(m.Groups[1].Value) : 0;
            })
            .ToArray();

        if (sprites.Length == 0)
        {
            Debug.LogError($"❌ Puzzle sprite bulunamadı: Puzzle/{levelName}");
            return;
        }

        // 3) Shuffle
        List<Sprite> shuffled = new List<Sprite>(sprites);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int r = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[r]) = (shuffled[r], shuffled[i]);
        }

        // 4) Instantiate & doğru konum bilgisini ata
        for (int i = 0; i < shuffled.Count; i++)
        {
            var go = Instantiate(piecePrefab, puzzleGrid);
            go.name = $"Piece_{i}";
            var img = go.GetComponent<Image>();
            img.sprite = shuffled[i];
            img.color  = Color.white;

            // bu sprite hangi hücrede olmalı?
            int correctIdx = System.Array.FindIndex(sprites, s => s.name == shuffled[i].name);
            var pp = go.GetComponent<PuzzlePiece>();
            pp.correctIndex = correctIdx;
        }

        // 5) Hücre boyutlarını ayarla (opsiyonel)
        AdjustGridCellSize(sprites.Length);

        // 6) PuzzleManager'a haber ver
        PuzzleManager.Instance.totalPieces = sprites.Length;
    }

    void AdjustGridCellSize(int pieceCount)
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(pieceCount));
        var gridRect = puzzleGrid.GetComponent<RectTransform>();
        var layout   = puzzleGrid.GetComponent<GridLayoutGroup>();

        float padX = layout.padding.left + layout.padding.right;
        float padY = layout.padding.top  + layout.padding.bottom;
        float spX  = layout.spacing.x * (gridSize - 1);
        float spY  = layout.spacing.y * (gridSize - 1);

        float availW = gridRect.rect.width  - padX - spX;
        float availH = gridRect.rect.height - padY - spY;

        layout.cellSize = new Vector2(availW / gridSize, availH / gridSize);
    }
}
