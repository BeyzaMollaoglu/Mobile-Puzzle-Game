using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleManagerUI : MonoBehaviour
{
    public Texture2D sourceTexture;
    public int rows = 5;
    public int columns = 10;

    public GameObject piecePrefab;
    public Transform gridParent;

    private List<PuzzlePieceUI> pieces = new List<PuzzlePieceUI>();
    private PuzzlePieceUI firstSelected = null;

    void Start()
    {
        SliceAndCreate();
        Shuffle();
    }

    void SliceAndCreate()
    {
        pieces.Clear();

        int pieceWidth = sourceTexture.width / columns;
        int pieceHeight = sourceTexture.height / rows;

        for (int i = 0; i < rows * columns; i++)
        {
            int x = i % columns;
            int y = i / columns;

            Rect rect = new Rect(x * pieceWidth, y * pieceHeight, pieceWidth, pieceHeight);
            Sprite sprite = Sprite.Create(sourceTexture, rect, new Vector2(0.5f, 0.5f), 100f);

            GameObject obj = Instantiate(piecePrefab, gridParent);
            PuzzlePieceUI piece = obj.GetComponent<PuzzlePieceUI>();
            piece.Setup(this, sprite, i);

            pieces.Add(piece);
        }
    }

    void Shuffle()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int rand = Random.Range(0, pieces.Count);
            SwapSprites(pieces[i], pieces[rand]);
        }
    }

    public void OnPieceClicked(PuzzlePieceUI clicked)
    {
        if (firstSelected == null)
        {
            firstSelected = clicked;
            clicked.image.color = Color.yellow; // seçili efekti
        }
        else
        {
            SwapSprites(firstSelected, clicked);
            firstSelected.image.color = Color.white;
            firstSelected = null;
        }
    }

    void SwapSprites(PuzzlePieceUI a, PuzzlePieceUI b)
    {
        Sprite temp = a.image.sprite;
        a.image.sprite = b.image.sprite;
        b.image.sprite = temp;
    }
}
