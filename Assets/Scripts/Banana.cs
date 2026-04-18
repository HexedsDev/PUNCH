using UnityEngine;

public class Banana : MonoBehaviour
{
    public int scoreValue = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(scoreValue);

            Destroy(gameObject);
        }
    }
}
