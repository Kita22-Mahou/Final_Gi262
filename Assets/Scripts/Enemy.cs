using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private string enemyName;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float attackCooldown = 1f;

    [SerializeField] private int hpPerWave = 5;        // HP เพิ่มต่อ wave
    [SerializeField] private int wavesPerDamage = 2;   // ทุกกี่ wave ดาเมจ +1

    private Transform target;
    private float attackTimer;

    [Header("Item Setting")]
    [SerializeField] private int[] items;              // index ของ item ที่ดรอปได้
    [SerializeField] private GameObject[] itemPrefabs; // 0 = Heal, 1 = AttackSpeed

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
        int roll = Random.Range(0, 100);
        int count = Mathf.Min(items.Length, itemPrefabs.Length);
        int cumulative = 0;

        Player player = target.GetComponent<Player>();
        if (player.CurrentHP <= 20 && !player.isPlayerLowHPGuaranteeDropped)
        {
            Debug.Log("LowHP Guarantee Dropped");
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
            target.GetComponent<Player>().isPlayerLowHPGuaranteeDropped = true;
            return;
        }

        for (int i = 0; i < count; i++)
        {
            cumulative += items[i];                            
            if (roll < cumulative)
            {
                Instantiate(itemPrefabs[i], transform.position, Quaternion.identity);
                return;
            }
        }
    }

    protected override void Die()
    {
        EnemyManager.Instance.Unregister(this);
        DropItem();
        target.GetComponent<Player>().AddScore(scoreValue);
        base.Die();
    }
}