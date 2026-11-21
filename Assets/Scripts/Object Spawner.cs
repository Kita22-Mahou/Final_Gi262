
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ObjectSpawner : MonoBehaviour
{
    private Wave wave;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private int spawnRange = 15;
    [SerializeField] private int wantedAmount;
    [SerializeField] private int spawnedObjectAmount = 0;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private bool previousWaveState;

    private void Start()
    {
        wave = GetComponent<Wave>();
        previousWaveState = wave.isWaveStopped;
    }

    private void Update()
    {
        // ตรวจจับการเปลี่ยนจาก Wave ทำงาน -> Wave หยุด
        if (!previousWaveState && wave.isWaveStopped)
        {
            ResetSpawner();
        }

        // อัปเดตสถานะ Wave
        previousWaveState = wave.isWaveStopped;

        // Spawn Object เมื่อ Wave หยุด
        if (wave.isWaveStopped && spawnedObjectAmount < wantedAmount)
        {
            SpawnObjectNew();
        }
    }

    private void SpawnObjectNew()
    {
        int randomPoint = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPoint = new Vector2(spawnPoints[randomPoint].position.x , spawnPoints[randomPoint].position.y);

        int objectVariant = Random.Range(0, objects.Length);

        GameObject obj = Instantiate(
            objects[objectVariant],
            spawnPoint,
            objects[objectVariant].transform.rotation,
            transform.parent
        );

        spawnedObjects.Add(obj);
        spawnedObjectAmount++;
    }

    private void SpawnObject()
    {
        int posX = Random.Range(-spawnRange, spawnRange+1);
        int posY = Random.Range(-spawnRange, spawnRange+1);

        int objectVariant = Random.Range(0, objects.Length);
        Vector2 pos = new Vector2(posX, posY);

        GameObject obj = Instantiate(
            objects[objectVariant],
            pos,
            objects[objectVariant].transform.rotation,
            transform.parent
        );

        spawnedObjects.Add(obj);
        spawnedObjectAmount++;
    }

    private void DestroyOldObject()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        spawnedObjects.Clear();
    }

    private void ResetSpawner()
    {
        spawnedObjectAmount = 0;
        DestroyOldObject();
    }
}
