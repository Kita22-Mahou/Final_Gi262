using UnityEngine;
using System.Collections;

public class Player : Character
{
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int score;
    [SerializeField] private GameObject bulletPrefab;
    private Rigidbody2D rb;

    private Vector2 moveDir;
    private Vector2 lastDir = Vector2.right;
    private float shootTimer;

    private float buffTimeLeft = 0f;
    private float buffAmount = 0f;
    [SerializeField] private float maxBuffTime = 15f;

    [Header("Related to Item")]
    public bool isPlayerLowHPGuaranteeDropped = true;
    [SerializeField] private float guaranteeCooldown;
    [SerializeField] private float time;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
        GuaranteeHealDropChecker();

        // ยิงอัตโนมัติ
        shootTimer += Time.deltaTime;
        if (shootTimer >= attackSpeed)
        {
            shootTimer = 0f;
            Shoot();
        }

        if (buffTimeLeft > 0f)
        {
            buffTimeLeft -= Time.deltaTime;
            if (buffTimeLeft <= 0f)
            {
                buffTimeLeft = 0f;
                attackSpeed += buffAmount;
                buffAmount = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDir * speed;
    }

    public void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveDir = new Vector2(x, y).normalized;

        if (moveDir != Vector2.zero)
            lastDir = moveDir;
    }

    public void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        b.GetComponent<Bullet>().Init(lastDir, damage, this);
    }

    //ยิงไปที่ Enemy ที่ใกล้ที่สุด
    //public void Shoot()
    //{
    //    Enemy nearest = EnemyManager.Instance.FindNearest(transform.position);
    //    if (nearest == null) return;   // ไม่มี enemy ก็ไม่ยิง

    //    Vector2 dir = (nearest.transform.position - transform.position).normalized;
    //    GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    //    b.GetComponent<Bullet>().Init(dir, damage, this);
    //}

    public void Heal(int healAmout)
    {
        currentHP = Mathf.Min(currentHP + healAmout, MaxHP);
        Debug.Log("Heal! HP: " + currentHP);
    }

    public void AddSpeed(float speedAmout, float duration)
    {
        if (buffTimeLeft <= 0f)
        {
            buffAmount = speedAmout;
            attackSpeed -= buffAmount;
        }
        buffTimeLeft = Mathf.Min(buffTimeLeft + duration, maxBuffTime);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    protected override void Die()
    {
        Debug.Log("Game Over! Score: " + score);
        base.Die();
    }

    private void GuaranteeHealDropChecker()
    {
        if (time < guaranteeCooldown)
        {
            time += Time.deltaTime;
            return;
        }

        if (currentHP < 20 && isPlayerLowHPGuaranteeDropped)
        {
            Debug.Log("Reset Cooldown");
            isPlayerLowHPGuaranteeDropped = false;
            time = 0;
        }
    }
}