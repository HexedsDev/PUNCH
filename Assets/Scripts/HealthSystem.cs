using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    [Header("Lives")]
    public int maxLives = 3;
    private int currentLives;

    [Header("UI")]
    public TextMeshProUGUI livesText;

    private float invincibleTimer = 0f;
    public float invincibleDuration = 0.5f;
    private bool isGameOver = false;

    void Start()
    {
        currentLives = maxLives;
        DrawLivesUI();
    }

    void Update()
    {
        if (invincibleTimer > 0f) invincibleTimer -= Time.deltaTime;
    }

    public void TakeDamage()
    {
        if (invincibleTimer > 0f || isGameOver) return;
        currentLives--;
        invincibleTimer = invincibleDuration;
        DrawLivesUI();

        if (currentLives <= 0)
        {
            StartCoroutine(GameOverRoutine());
        }
    }

    private IEnumerator GameOverRoutine()
    {
        isGameOver = true;
        if (GameUI.Instance != null)
        {
            GameUI.Instance.ShowGameOver();
        }

        // Disable player movement and attacking to fully represent death
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<AutoAttack>().enabled = false;

        // Wait 3 seconds to let them read the text
        yield return new WaitForSeconds(3f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DrawLivesUI()
    {
        if (livesText == null) return;
        string h = "";
        for (int i = 0; i < currentLives; i++) h += "♥ ";
        livesText.text = "Vidas: " + h;
    }
}
