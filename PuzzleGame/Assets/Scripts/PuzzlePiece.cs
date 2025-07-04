using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int correctIndex;
    public int currentIndex;
    public Image image;

    private Transform parent;
    private Vector3 originalPos;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.position;
        parent = transform.parent;
        transform.SetParent(parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;

        if (target != null && target.GetComponent<PuzzlePiece>() != null)
        {
            PuzzlePiece other = target.GetComponent<PuzzlePiece>();

            Sprite tempSprite = other.image.sprite;
            int tempCorrectIndex = other.correctIndex;
            int tempCurrentIndex = other.currentIndex;

            other.image.sprite = image.sprite;
            other.correctIndex = this.correctIndex;
            other.currentIndex = this.currentIndex;

            image.sprite = tempSprite;
            correctIndex = tempCorrectIndex;
            currentIndex = tempCurrentIndex;
        }

        transform.SetParent(parent);
        transform.position = originalPos;

        PuzzleManager.Instance.CheckWin();
    }
}
