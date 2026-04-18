using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public float fireRate = 0.8f;
    public float detectionRange = 20f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Transform nearest = FindNearestEnemy();
            if (nearest != null)
            {
                FireAt(nearest);
                fireTimer = fireRate;
            }
        }
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null;
        float bestDist = detectionRange;

        foreach (GameObject e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = e.transform;
            }
        }
        return best;
    }

    void FireAt(Transform target)
    {
        if (projectilePrefab == null) return;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null) p.SetTarget(target);
    }
}
