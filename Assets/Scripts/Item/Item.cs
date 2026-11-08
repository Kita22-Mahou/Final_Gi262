using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name;
    protected abstract void Use(Player player);

    [SerializeField] private float lifeTime = 10f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p != null)
        {
            Use(p);
            Destroy(gameObject);  
        }
    }
}
