using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 2f;
    public float damageCooldown = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private float damageTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }

    void Update()
    {
        if (damageTimer > 0f) damageTimer -= Time.deltaTime;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && damageTimer <= 0f)
        {
            HealthSystem hs = col.gameObject.GetComponent<HealthSystem>();
            if (hs != null)
            {
                hs.TakeDamage();
                damageTimer = damageCooldown;
            }
        }
    }
}
