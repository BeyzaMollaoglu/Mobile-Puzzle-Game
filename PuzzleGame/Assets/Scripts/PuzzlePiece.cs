using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int correctIndex;

    private static PuzzlePiece _selected;
    private Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_selected == null)
        {
            _selected = this;
            _img.color = Color.yellow;
            return;
        }

        if (_selected == this)
        {
            _selected._img.color = Color.white;
            _selected = null;
            return;
        }

        int myIndex = transform.GetSiblingIndex();
        int otherIndex = _selected.transform.GetSiblingIndex();

        transform.SetSiblingIndex(otherIndex);
        _selected.transform.SetSiblingIndex(myIndex);

        _selected._img.color = Color.white;
        _img.color = Color.white;
        _selected = null;

        PuzzleManager.Instance.CheckWin();
    }
}
