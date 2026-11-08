using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private string enemyName;
    [SerializeField] private int[] items;              // index ของ item ที่ดรอปได้
    [SerializeField] private GameObject[] itemPrefabs; // 0 = Heal, 1 = AttackSpeed
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float attackCooldown = 1f;

    [SerializeField] private int hpPerWave = 5;        // HP เพิ่มต่อ wave
    [SerializeField] private int wavesPerDamage = 2;   // ทุกกี่ wave ดาเมจ +1

    private Transform target;
    private float attackTimer;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        EnemyManager.Instance.Register(this);
    }

    void Update()
    {
        if (target == null) return;
        ChaseMovement();
        attackTimer += Time.deltaTime;
    }

    public void ChaseMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void Attack()
    {
        if (attackTimer < attackCooldown) return;
        attackTimer = 0f;
        target.GetComponent<Player>().TakeDamage(damage);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            Attack();
    }

    public void ScaleByWave(int wave)
    {
        int bonus = wave - 1;                  // wave 1 = ค่าปกติ ไม่เพิ่ม
        MaxHP += bonus * hpPerWave;
        currentHP = MaxHP;                     // เติมเลือดให้เต็มตาม MaxHP ใหม่
        damage += wavesPerDamage;    
    }

    public void DropItem()
    {
        if (items.Length == 0) return;
        int idx = items[Random.Range(0, items.Length)];
        Instantiate(itemPrefabs[idx], transform.position, Quaternion.identity);
    }

    protected override void Die()
    {
        EnemyManager.Instance.Unregister(this);
        DropItem();
        target.GetComponent<Player>().AddScore(scoreValue);
        base.Die();
    }
}