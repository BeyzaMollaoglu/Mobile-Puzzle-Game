// PuzzleManagerUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleManagerUI : MonoBehaviour
{
    [Header("Puzzle Ayarları")]
    public Texture2D sourceTexture;
    public int rows = 5;
    public int columns = 10;

    [Header("Prefab & UI")]
    public GameObject piecePrefab;  // İçinde Image + PuzzlePieceUI var
    public Transform gridParent;    // GridLayoutGroup, Start Corner = Upper Left

    private List<PuzzlePieceUI> pieces = new List<PuzzlePieceUI>();
    private PuzzlePieceUI firstSelected;

    void Start()
    {
        Debug.Log("▶️ CreatePieces başlatılıyor");
        CreatePieces();
        ShufflePieces();
    }

    void CreatePieces()
    {
        // Önceki parçaları temizle
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        pieces.Clear();

        int w = sourceTexture.width  / columns;
        int h = sourceTexture.height / rows;

        // Upper Left referanslı satır/sütun döngüsü
        for (int row = 0; row < rows; row++)
        {
            int invRow = (rows - 1) - row; // Texture2D alt-sol orijini düzeltmesi
            for (int col = 0; col < columns; col++)
            {
                int id = row * columns + col; // 0..49
                Debug.Log($"🌀 Loop: row={row}, col={col}, id={id}");

                Rect rect = new Rect(col * w, invRow * h, w, h);
                Sprite spr = Sprite.Create(sourceTexture, rect, new Vector2(0.5f, 0.5f), 100f);

                GameObject go = Instantiate(piecePrefab, gridParent);
                go.name = $"Piece_{id}";
                PuzzlePieceUI piece = go.GetComponent<PuzzlePieceUI>();
                piece.Setup(this, spr, id);
                pieces.Add(piece);
            }
        }
    }

    void ShufflePieces()
    {
        var rnd = new System.Random();
        for (int i = pieces.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(0, i + 1);
            pieces[i].SwapWith(pieces[j]);
        }
    }

    public void OnPieceClicked(PuzzlePieceUI clicked)
    {
        if (firstSelected == null)
        {
            firstSelected = clicked;
            firstSelected.image.color = Color.yellow;
        }
        else
        {
            clicked.image.color       = Color.white;
            firstSelected.image.color = Color.white;

            firstSelected.SwapWith(clicked);
            firstSelected = null;

            Debug.Log("Takas yapıldı, kontrol ediliyor...");
            CheckForWin();
        }
    }

    void CheckForWin()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (!pieces[i].IsCorrect())
            {
                Debug.Log($"✖ Parça {i}: current={pieces[i].currentID}, correct={pieces[i].correctID}");
                return;
            }
        }

        Debug.Log("🎉 Kazandın!");
    }
}
