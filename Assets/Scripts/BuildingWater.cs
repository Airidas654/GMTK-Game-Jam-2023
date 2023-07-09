using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BuildingWater : Building
{
    public static Pump Instance = null;

    [SerializeField] GameObject dropPrefab;
    [SerializeField] int maxDropletCount = 5;
    [SerializeField] int waterAmount = 3;
    [SerializeField] float disperseDist = 0.5f;

    public ObjectPool<GameObject> drops { get; private set; }

    private new void Start()
    {
        base.Start();
        drops = new ObjectPool<GameObject>(CreateDrop, GetDrop, ReleaseDrop);
        spawnVal = dropSpawnTime;
    }

    public bool stopped = false;
    [SerializeField] float dropSpawnTime = 2f;

    float spawnVal;

    int dropletCount;
    private new void Update()
    {
        base.Update();
        waterVal = 1f;
        if (!stopped && grown && !dead && dropletCount < maxDropletCount)
        {
            spawnVal -= Time.deltaTime;
            if (spawnVal <= 0)
            {
                dropletCount++;
                GameObject obj = drops.Get();
                obj.transform.position = transform.position;
                obj.GetComponent<PickableDrop>().Reset(drops,waterAmount, disperseDist);
                spawnVal = dropSpawnTime;
            }
        }
    }

    GameObject CreateDrop()
    {
        GameObject obj = Instantiate(dropPrefab);
        return obj;
    }

    void GetDrop(GameObject obj)
    {
        obj.SetActive(true);
    }

    void ReleaseDrop(GameObject obj)
    {
        dropletCount--;
        obj.SetActive(false);
    }
}
