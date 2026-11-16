using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 dir;
    [SerializeField] private int dmg;
    [SerializeField] private float bulletSpeed = 1;
    private Player owner;

    public void Init(Vector2 direction, int damage, Player p)
    {
        dir = direction;
        dmg = damage;
        owner = p;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(dir * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy e = col.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(dmg);
            Destroy(gameObject);
        }
    }
}