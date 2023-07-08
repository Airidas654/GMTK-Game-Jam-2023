using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class Pump : MonoBehaviour
{
    public static Pump Instance = null;

    [SerializeField] GameObject dropPrefab;
    [SerializeField] int maxDropletCount = 5;
    public ObjectPool<GameObject> drops { get; private set; }

    private void Start()
    {
        drops = new ObjectPool<GameObject>(CreateDrop,GetDrop, ReleaseDrop);
        spawnVal = dropSpawnTime;

        Transform obj = transform.GetChild(0);
        obj.DOMoveY(obj.transform.position.y + 0.1f, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
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

    int dropletCount;
    private void Update()
    {
        if (!stopped && dropletCount < maxDropletCount)
        {
            spawnVal -= Time.deltaTime;
            if (spawnVal <= 0)
            {
                dropletCount++;
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
        dropletCount--;
        obj.SetActive(false);
    }
}
