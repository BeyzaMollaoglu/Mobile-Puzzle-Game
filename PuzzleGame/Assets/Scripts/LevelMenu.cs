using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void LoadLevelByNumber(int levelNumber)
    {
        LevelData.selectedLevel = "level" + levelNumber;
        SceneManager.LoadScene("PuzzleScene");
    }
}
