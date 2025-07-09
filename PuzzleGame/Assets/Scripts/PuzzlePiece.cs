using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    public int correctIndex;
    public int currentIndex;
    public Image image;

    private static PuzzlePiece selectedPiece = null;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedPiece == null)
        {
            // İlk seçilen parça
            selectedPiece = this;
            image.color = Color.yellow;
        }
        else if (selectedPiece == this)
        {
            // Aynı parçaya iki kez tıklandı → seçimi iptal et
            image.color = Color.white;
            selectedPiece = null;
        }
        else
        {
            // İkinci parça seçildi → takas yap
            SwapWith(selectedPiece);
            image.color = Color.white;
            selectedPiece.image.color = Color.white;
            selectedPiece = null;

            // Kontrol et
            PuzzleManager.Instance.CheckWin();
        }
    }

    void SwapWith(PuzzlePiece other)
    {
        // Sprite takası
        Sprite tempSprite = image.sprite;
        image.sprite = other.image.sprite;
        other.image.sprite = tempSprite;

        // ✅ currentIndex takası
        int temp = currentIndex;
        currentIndex = other.currentIndex;
        other.currentIndex = temp;

        // ❌ correctIndex değişmez!
    }

    public bool IsInCorrectPosition()
    {
        return correctIndex == currentIndex;
    }
}
