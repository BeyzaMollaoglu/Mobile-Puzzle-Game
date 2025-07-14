using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public PuzzleSpawner spawner;

    public void LevelButton()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
