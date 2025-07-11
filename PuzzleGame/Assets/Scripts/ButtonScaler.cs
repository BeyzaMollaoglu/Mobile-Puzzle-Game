using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ButtonScaler : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad = "PuzzleScene";  // yüklenecek sahne adı
    public float pressScale = 0.8f;             // ne kadar küçülsün
    public float animDuration = 0.2f;           // animasyon toplam süresi
    
    Button uiButton;
    Transform tf;

    void Awake()
    {
        uiButton = GetComponent<Button>();
        tf = transform;
        uiButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // butonun tıklamasını kilitle (bir daha tıklamasın)
        uiButton.interactable = false;
        // animasyonu başlat
        StartCoroutine(DoPressAnimation());
    }

    IEnumerator DoPressAnimation()
    {
        // 1) Küçülme
        float half = animDuration * 0.5f;
        Vector3 start = Vector3.one;
        Vector3 target = Vector3.one * pressScale;
        for (float t = 0; t < half; t += Time.unscaledDeltaTime)
        {
            tf.localScale = Vector3.Lerp(start, target, t / half);
            yield return null;
        }
        tf.localScale = target;

        // 2) Geri büyüme
        for (float t = 0; t < half; t += Time.unscaledDeltaTime)
        {
            tf.localScale = Vector3.Lerp(target, start, t / half);
            yield return null;
        }
        tf.localScale = start;

        // 3) Sahneyi yükle
        SceneManager.LoadScene(sceneToLoad);
    }
}
