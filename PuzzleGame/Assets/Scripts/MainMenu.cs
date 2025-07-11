using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Animator playButtonAnimator; // <-- Animator referansı

    public void StartGame()
    {
        playButtonAnimator.SetTrigger("Pressed");

        // Animator’daki animasyon süresine göre bekle (örneğin 0.3 saniye)
        StartCoroutine(LoadSceneDelayed(0.3f));
    }

    IEnumerator LoadSceneDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("PuzzleScene");
    }

    public void LevelButton()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
