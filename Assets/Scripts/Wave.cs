using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave : MonoBehaviour
{
    [SerializeField] private int waveCount = 0;
    [SerializeField] private int enemyAmout = 5;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private int creepAmout = 2;

    [SerializeField] private GameObject[] enemyPrefabs;

    // ขนาด arena (ครึ่งหนึ่งของความกว้าง/ความสูง) และระยะห่างนอกขอบ
    [SerializeField] private float arenaHalfWidth = 9f;
    [SerializeField] private float arenaHalfHeight = 5f;
    [SerializeField] private float spawnMargin = 2f;

    private Queue<GameObject> spawnQueue = new Queue<GameObject>();

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            waveCount++;
            Debug.Log("Wave " + waveCount + " : " + enemyAmout + " enemies");

            // 1) เติมคิว: สุ่มชนิด enemy ต่อท้ายคิว
            for (int i = 0; i < enemyAmout; i++)
            {
                int idx = Random.Range(0, enemyPrefabs.Length);
                spawnQueue.Enqueue(enemyPrefabs[idx]);
            }

            // 2) ปล่อยออกจากคิวทีละตัว
            while (spawnQueue.Count > 0)
            {
                RandomSpawn();
                yield return new WaitForSeconds(delay);
            }

            // 3) รอให้ผู้เล่นกำจัดจนหมด
            while (EnemyManager.Instance.Count > 0)
                yield return null;

            // 4) เตรียม wave ต่อไป
            enemyAmout += creepAmout;
            yield return new WaitForSeconds(2f);   // พักหายใจ
        }
    }

    public void RandomSpawn()
    {
        GameObject prefab = spawnQueue.Dequeue();   // เอาตัวหน้าสุดออกจากคิว

        float w = arenaHalfWidth + spawnMargin;
        float h = arenaHalfHeight + spawnMargin;
        Vector2 pos = Vector2.zero;

        int side = Random.Range(0, 4);              // 0=บน 1=ล่าง 2=ซ้าย 3=ขวา
        switch (side)
        {
            case 0: pos = new Vector2(Random.Range(-w, w), h); break;
            case 1: pos = new Vector2(Random.Range(-w, w), -h); break;
            case 2: pos = new Vector2(-w, Random.Range(-h, h)); break;
            case 3: pos = new Vector2(w, Random.Range(-h, h)); break;
        }

        Instantiate(prefab, pos, Quaternion.identity);
    }
}