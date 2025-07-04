using UnityEngine;

public class LevelController : MonoBehaviour
{
    public PuzzleSpawner spawner;

    public void LoadLevel2()
    {
        spawner.LoadLevel("level2");
    }
}
