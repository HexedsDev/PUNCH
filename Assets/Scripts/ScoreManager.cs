using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    private int score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        DrawScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        DrawScoreUI();
    }

    private void DrawScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
