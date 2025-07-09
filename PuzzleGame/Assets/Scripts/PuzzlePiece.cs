using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int correctIndex; // bu sprite hangi hücrede olmalı

    private static PuzzlePiece _selected;
    private Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 1) Hiç parça seçili değil → bu parçayı seç
        if (_selected == null)
        {
            _selected = this;
            _img.color = Color.yellow;
            return;
        }

        // 2) Aynı parçaya tıklanırsa seçimi iptal et
        if (_selected == this)
        {
            _selected._img.color = Color.white;
            _selected = null;
            return;
        }

        // 3) Farklı bir parça zaten seçili → ikisini yer değiştir
        int myIndex    = transform.GetSiblingIndex();
        int otherIndex = _selected.transform.GetSiblingIndex();

        // swap sibling index
        transform.SetSiblingIndex(otherIndex);
        _selected.transform.SetSiblingIndex(myIndex);

        // renkleri temizle
        _selected._img.color = Color.white;
        _img.color            = Color.white;
        _selected             = null;

        // kazanma kontrolü
        PuzzleManager.Instance.CheckWin();
    }
}
