using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private const string PlayerNameTag = "Player";
    private const float AfterLevelExitDelay = 1f;

    [SerializeField] private List<SceneAsset> _levels;
    [SerializeField] private SceneAsset currentLevel;

    public SceneAsset CurrentLevel => currentLevel;
    private static int _levelNumber = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ExitLevel(collision);
    }

    private void SetCurrentLevel()
    {
        if (_levelNumber < _levels.Count - 1)
        {
            _levelNumber++;
        }
        else
        {
            _levelNumber = 0;
        }
        Debug.Log(_levelNumber);
        currentLevel = _levels[_levelNumber];
    }

    private void ExitLevel(Collider2D collision)
    {
        if (collision.CompareTag(PlayerNameTag))
        {
            Invoke(nameof(GetNextLevel), AfterLevelExitDelay);
        }
    }

    private void GetNextLevel()
    {
        SetCurrentLevel();
        SceneManager.LoadScene(currentLevel.name);
    }
}
