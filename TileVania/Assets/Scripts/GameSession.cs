using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public sealed class GameSession : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    public static GameSession Instance;

    private int _score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log(this);
        }
    }  
    
    private void Start()
    {
        SetScoreText();
    }

    private void SetScoreText()
    {
        _scoreText.text = _score.ToString();
    }

    public void AddScore()
    {
        _score++;
        SetScoreText();
    }
}
