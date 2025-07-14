using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSpawner : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject piecePrefab;
    public Transform puzzleGrid;

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
        foreach (Transform c in puzzleGrid) Destroy(c.gameObject);

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

        List<Sprite> shuffled = new List<Sprite>(sprites);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int r = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[r]) = (shuffled[r], shuffled[i]);
        }

        for (int i = 0; i < shuffled.Count; i++)
        {
            var go = Instantiate(piecePrefab, puzzleGrid);
            go.name = $"Piece_{i}";
            var img = go.GetComponent<Image>();
            img.sprite = shuffled[i];
            img.color = Color.white;

            int correctIdx = System.Array.FindIndex(sprites, s => s.name == shuffled[i].name);
            var pp = go.GetComponent<PuzzlePiece>();
            pp.correctIndex = correctIdx;
        }

        AdjustGridCellSize(sprites.Length);

        PuzzleManager.Instance.totalPieces = sprites.Length;

 if (PuzzleManager.Instance != null && PuzzleManager.Instance.originalImage != null)
{
    Sprite fullImage = Resources.Load<Sprite>("FullPuzzleImages/" + levelName);
    if (fullImage != null)
    {
        PuzzleManager.Instance.originalImage.sprite = fullImage;
        Debug.Log("✅ Loaded full image for: " + levelName);
    }
    else
    {
        Debug.LogWarning("❌ Full image not found at Resources/FullPuzzleImages/" + levelName);
    }
}
    }

    void AdjustGridCellSize(int pieceCount)
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(pieceCount));
        var gridRect = puzzleGrid.GetComponent<RectTransform>();
        var layout = puzzleGrid.GetComponent<GridLayoutGroup>();

        float padX = layout.padding.left + layout.padding.right;
        float padY = layout.padding.top + layout.padding.bottom;
        float spX = layout.spacing.x * (gridSize - 1);
        float spY = layout.spacing.y * (gridSize - 1);

        float availW = gridRect.rect.width - padX - spX;
        float availH = gridRect.rect.height - padY - spY;

        layout.cellSize = new Vector2(availW / gridSize, availH / gridSize);
    }
}
