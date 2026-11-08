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

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();

        // ยิงอัตโนมัติ
        shootTimer += Time.deltaTime;
        if (shootTimer >= attackSpeed)
        {
            shootTimer = 0f;
            Shoot();
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

    public void AddSpeed(int speedAmout, float duration)
    {
        StartCoroutine(AttackSpeedBuff(speedAmout, duration));
    }

    private IEnumerator AttackSpeedBuff(int speedAmout, float duration)
    {
        attackSpeed += speedAmout;
        yield return new WaitForSeconds(duration);
        attackSpeed -= speedAmout;
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
}