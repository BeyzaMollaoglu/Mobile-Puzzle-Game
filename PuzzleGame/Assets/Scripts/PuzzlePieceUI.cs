// PuzzlePieceUI.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePieceUI : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int correctID;
    [HideInInspector] public int currentID;
    [HideInInspector] public Image image;
    private PuzzleManagerUI manager;

    // Çağıran taraf: Setup(this, sprite, id)
    public void Setup(PuzzleManagerUI mgr, Sprite sprite, int id)
    {
        manager    = mgr;
        correctID  = id;
        currentID  = id;
        image      = GetComponent<Image>();
        image.sprite = sprite;

        Debug.Log($"[Setup] {gameObject.name} → correctID={correctID}, currentID={currentID}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.OnPieceClicked(this);
    }

    // İki parçayı hem sprite hem ID olarak takas eder
    public void SwapWith(PuzzlePieceUI other)
    {
        // Sprite takas
        var tmpSprite      = image.sprite;
        image.sprite       = other.image.sprite;
        other.image.sprite = tmpSprite;

        // ID takas
        var tmpID          = currentID;
        currentID          = other.currentID;
        other.currentID    = tmpID;
    }

    // Kazanma kontrolü
    public bool IsCorrect()
    {
        return currentID == correctID;
    }
}
