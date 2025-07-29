using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ButtonScaler : MonoBehaviour
{
    [Header("Settings")]
    public float pressScale = 0.8f;
    public float animDuration = 0.2f;

    [Header("Events")]
    public UnityEvent onAnimationComplete;

    private Button uiButton;
    private Transform tf;
    private bool isAnimating = false;

    void Awake()
    {
        uiButton = GetComponent<Button>();
        tf = transform;
        uiButton.onClick.AddListener(PlayAnimation);
    }

    void PlayAnimation()
    {
        if (!isAnimating)
            StartCoroutine(DoPressAnimation());
    }

    IEnumerator DoPressAnimation()
    {
        isAnimating = true;

        float half = animDuration * 0.5f;
        Vector3 start = Vector3.one;
        Vector3 target = Vector3.one * pressScale;

        for (float t = 0; t < half; t += Time.unscaledDeltaTime)
        {
            tf.localScale = Vector3.Lerp(start, target, t / half);
            yield return null;
        }
        tf.localScale = target;

        for (float t = 0; t < half; t += Time.unscaledDeltaTime)
        {
            tf.localScale = Vector3.Lerp(target, start, t / half);
            yield return null;
        }
        tf.localScale = start;

        // Dışarıya haber ver
        onAnimationComplete?.Invoke();
        isAnimating = false;
    }
}
