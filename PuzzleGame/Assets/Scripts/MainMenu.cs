using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("PuzzleScene");
    }

    public void LevelButton()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
