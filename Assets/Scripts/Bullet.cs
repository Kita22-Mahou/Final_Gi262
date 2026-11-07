using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 dir;
    private int dmg;
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
        transform.Translate(dir * 10f * Time.deltaTime);
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