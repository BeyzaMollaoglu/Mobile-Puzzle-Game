using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePieceUI : MonoBehaviour, IPointerClickHandler
{
    public int index; // Listenin içindeki konumu
    public Image image;
    private PuzzleManagerUI manager;

    public void Setup(PuzzleManagerUI mgr, Sprite sprite, int i)
    {
        manager = mgr;
        index = i;
        image = GetComponent<Image>();
        image.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.OnPieceClicked(this);
    }
}
