using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


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

    // Disable interaction during animation
    StartCoroutine(AnimateSwap(_selected));
    
    _selected._img.color = Color.white;
    _img.color = Color.white;
    _selected = null;
}
private IEnumerator AnimateSwap(PuzzlePiece other)
{
    Transform t1 = transform;
    Transform t2 = other.transform;

    Vector3 startPos1 = t1.position;
    Vector3 startPos2 = t2.position;

    float duration = 0.2f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        float t = elapsed / duration;
        t1.position = Vector3.Lerp(startPos1, startPos2, t);
        t2.position = Vector3.Lerp(startPos2, startPos1, t);
        elapsed += Time.deltaTime;
        yield return null;
    }

    // Snap to final position
    t1.position = startPos2;
    t2.position = startPos1;

    // Swap their sibling indexes (UI reorder)
    int myIndex = t1.GetSiblingIndex();
    int otherIndex = t2.GetSiblingIndex();
    t1.SetSiblingIndex(otherIndex);
    t2.SetSiblingIndex(myIndex);

    // Optional: re-layout the grid to fix spacing
    LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);

    // Check if puzzle is solved
    PuzzleManager.Instance.CheckWin();
}

}
