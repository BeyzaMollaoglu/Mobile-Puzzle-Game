using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    public int correctIndex; // ✅ Eklendi
    public int currentIndex; // ✅ Eklendi

    public Image image;
    public static PuzzlePiece selectedPiece;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedPiece == null)
        {
            selectedPiece = this;
            image.color = Color.yellow;
        }
        else if (selectedPiece == this)
        {
            selectedPiece.image.color = Color.white;
            selectedPiece = null;
        }
        else
        {
            SwapWith(selectedPiece);
            selectedPiece.image.color = Color.white;
            image.color = Color.white;
            selectedPiece = null;

            PuzzleManager.Instance.CheckWin();
        }
    }

    public void SwapWith(PuzzlePiece other)
    {
        // Görselleri takas et
        Sprite tempSprite = image.sprite;
        image.sprite = other.image.sprite;
        other.image.sprite = tempSprite;

        // 🔁 Index takası
        int tempIndex = currentIndex;
        currentIndex = other.currentIndex;
        other.currentIndex = tempIndex;
    }

    public bool IsInCorrectPosition()
    {
        return currentIndex == correctIndex;
    }
}
