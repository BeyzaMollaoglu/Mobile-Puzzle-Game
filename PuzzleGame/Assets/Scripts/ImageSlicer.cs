using UnityEngine;
using System.Collections.Generic;

public class ImageSlicer : MonoBehaviour
{
    public Texture2D sourceTexture;
    public int rows = 5;
    public int columns = 10;
    public GameObject piecePrefab;
    public Vector2 startPosition = new Vector2(-5, 3);
    public float spacing = 1.1f;
    public Transform gridParent; 
    public List<Sprite> SlicedSprites = new List<Sprite>();

    void Start()
    {
        SliceImage();
        InstantiateSlices();
    }

    void SliceImage()
    {
        if (sourceTexture == null)
        {
            Debug.LogError("Source Texture atanmadı.");
            return;
        }

        SlicedSprites.Clear();

        int pieceWidth = sourceTexture.width / columns;
        int pieceHeight = sourceTexture.height / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Rect rect = new Rect(x * pieceWidth, y * pieceHeight, pieceWidth, pieceHeight);
                Sprite newSprite = Sprite.Create(sourceTexture, rect, new Vector2(0.5f, 0.5f), 100f);
                SlicedSprites.Add(newSprite);
            }
        }

        Debug.Log($"Toplam {SlicedSprites.Count} parça oluşturuldu.");
    }


    void InstantiateSlices()
    {
    if (piecePrefab == null || gridParent == null)
    {
        Debug.LogError("PiecePrefab ya da GridParent atanmadı!");
        return;
    }

    for (int i = 0; i < SlicedSprites.Count; i++)
    {
        GameObject piece = Instantiate(piecePrefab, gridParent);
        piece.GetComponent<UnityEngine.UI.Image>().sprite = SlicedSprites[i];
        piece.name = $"Piece_{i}";
    }
    }
}

