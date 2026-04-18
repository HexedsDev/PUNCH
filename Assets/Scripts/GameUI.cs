using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Creates the game HUD at runtime (Canvas + TextMeshPro labels for Lives, Score, and Game Over).
/// </summary>
public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    private TextMeshProUGUI livesLabel;
    private TextMeshProUGUI scoreLabel;
    private TextMeshProUGUI waveLabel;
    private TextMeshProUGUI gameOverLabel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        BuildCanvas();
    }

    void BuildCanvas()
    {
        // Root canvas
        GameObject canvasGO = new GameObject("GameCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // ── TOP BAR ──────────────────────────────────────────
        GameObject topBar = MakePanel(canvasGO.transform, "TopBar",
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1),
            new Vector2(0, -70), new Vector2(0, 0));
        topBar.GetComponent<Image>().color = new Color(0, 0, 0, 0.55f);

        livesLabel = MakeLabel(topBar.transform, "LivesLabel", "❤ ❤ ❤", 36,
            new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f),
            new Vector2(20, 0));
        livesLabel.color = new Color(1f, 0.2f, 0.2f);

        waveLabel = MakeLabel(topBar.transform, "WaveLabel", "Wave 1", 30,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero);
        waveLabel.color = Color.white;
        waveLabel.alignment = TextAlignmentOptions.Center;

        // ── BOTTOM BAR ────────────────────────────────────────
        GameObject bottomBar = MakePanel(canvasGO.transform, "BottomBar",
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0),
            new Vector2(0, 70), new Vector2(0, 0));
        bottomBar.GetComponent<Image>().color = new Color(0, 0, 0, 0.55f);

        scoreLabel = MakeLabel(bottomBar.transform, "ScoreLabel", "Score: 0", 36,
            new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f),
            new Vector2(20, 0));
        scoreLabel.color = new Color(1f, 0.95f, 0f);

        // ── GAME OVER PANEL ───────────────────────────────────
        GameObject gameOverPanel = MakePanel(canvasGO.transform, "GameOverPanel",
            new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        gameOverPanel.GetComponent<Image>().color = new Color(0.8f, 0, 0, 0.3f);
        gameOverPanel.SetActive(false); // Hidden initially

        gameOverLabel = MakeLabel(gameOverPanel.transform, "GameOverLabel", "HAS PERDIDO EL NIVEL 1\nReiniciando...", 80,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero);
        gameOverLabel.color = Color.white;
        gameOverLabel.alignment = TextAlignmentOptions.Center;
        gameOverLabel.rectTransform.sizeDelta = new Vector2(1200, 300);

        // Hook up to game systems
        HealthSystem hs = FindObjectOfType<HealthSystem>();
        if (hs != null) hs.livesText = livesLabel;

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null) sm.scoreText = scoreLabel;
    }

    GameObject MakePanel(Transform parent, string name,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
        Vector2 sizeDelta, Vector2 anchoredPos)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;
        return go;
    }

    TextMeshProUGUI MakeLabel(Transform parent, string name, string text, int size,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPos)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(400, 60);
        return tmp;
    }

    public void SetWaveText(int wave)
    {
        if (waveLabel != null) waveLabel.text = "Oleada " + wave;
    }

    public void ShowGameOver()
    {
        if (gameOverLabel != null && gameOverLabel.transform.parent != null)
        {
            gameOverLabel.transform.parent.gameObject.SetActive(true); // Activa el panel con el texto
        }
    }
}
