using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private List<Enemy> enemies = new List<Enemy>();
    public int Count => enemies.Count;

    void Awake() { Instance = this; }

    // O(1): เพิ่มท้าย List
    public void Register(Enemy e) { enemies.Add(e); }

    // O(n): Remove ต้องไล่หาตัวนั้นก่อน แล้วเลื่อนสมาชิกที่เหลือ
    public void Unregister(Enemy e) { enemies.Remove(e); }

    // Linear Search: O(n) ไล่ดูทุกตัว จำตัวที่ใกล้สุดไว้
    public Enemy FindNearest(Vector2 pos)
    {
        Enemy nearest = null;
        float minDist = float.MaxValue;
        foreach (Enemy e in enemies)
        {
            float d = Vector2.Distance(pos, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = e;
            }
        }
        return nearest;
    }

    // Selection Sort แบบหยุดก่อน: ได้ HP ต่ำสุด 'count' ตัว
    public List<Enemy> GetLowestHP(int count)
    {
        Enemy[] arr = enemies.ToArray();           // copy เพื่อไม่ให้ลำดับ List จริงเปลี่ยน
        count = Mathf.Min(count, arr.Length);

        for (int i = 0; i < count; i++)            // รอบนอกวนแค่ count รอบ ไม่ใช่ n-1
        {
            int minIndex = i;
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (arr[j].CurrentHP < arr[minIndex].CurrentHP)
                    minIndex = j;
            }
            (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
        }
        return new List<Enemy>(arr).GetRange(0, count);
    }
}