using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int speed;
    [SerializeField] protected int currentHP;
    public int CurrentHP => currentHP;
    [SerializeField] protected int MaxHP;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        currentHP = MaxHP;
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;
        Debug.Log(name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
