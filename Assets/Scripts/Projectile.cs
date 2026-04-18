using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
