using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleSpawner : MonoBehaviour
{
    public GameObject piecePrefab;
    public Transform puzzleGrid;

    [Header("Level Ayarları")]
    public string levelName = "level1"; // level1, level2, vs.

    void Start()
    {
        LoadLevel(levelName);
    }

    public void LoadLevel(string name)
    {
        ClearExistingPieces();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Puzzle1/" + name);

        if (sprites.Length == 0)
        {
            Debug.LogError("❌ " + name + " yüklenemedi. Sprite dilimlenmiş mi?");
            return;
        }

        List<Sprite> shuffled = new List<Sprite>(sprites);
        Shuffle(shuffled);

        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject piece = Instantiate(piecePrefab, puzzleGrid);
            piece.GetComponent<Image>().sprite = shuffled[i];

            PuzzlePiece pp = piece.GetComponent<PuzzlePiece>();
            pp.correctIndex = System.Array.IndexOf(sprites, shuffled[i]);
            pp.currentIndex = i;
        }

        PuzzleManager.Instance.totalPieces = sprites.Length;
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void ClearExistingPieces()
    {
        foreach (Transform child in puzzleGrid)
        {
            Destroy(child.gameObject);
        }
    }

    public class LevelController : MonoBehaviour
    {
        public PuzzleSpawner spawner;

        public void LoadLevel2()
        {
            spawner.LoadLevel("level2");
        }
    }
}
