using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pump : MonoBehaviour
{
    public static Pump Instance = null;

    [SerializeField] GameObject dropPrefab;
    public ObjectPool<GameObject> drops { get; private set; }

    private void Start()
    {
        drops = new ObjectPool<GameObject>(CreateDrop,GetDrop, ReleaseDrop);
        spawnVal = dropSpawnTime;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public bool stopped = false;
    [SerializeField] float dropSpawnTime = 2f;

    float spawnVal;

    private void Update()
    {
        if (!stopped)
        {
            spawnVal -= Time.deltaTime;
            if (spawnVal <= 0)
            {
                GameObject obj = drops.Get();
                obj.transform.position = transform.position;
                obj.GetComponent<PickableDrop>().Reset();
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
        obj.SetActive(false);
    }
}
