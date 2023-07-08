using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance = null;

    public ObjectPool<GameObject> shotTrailPool;
    [SerializeField] GameObject shotTrailPrefab;
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

    void Start()
    {
        shotTrailPool = new ObjectPool<GameObject>(OnTrailCreate, OnTrailGet, OnTrailRelease);
    }

    GameObject OnTrailCreate()
    {
        GameObject obj = Instantiate(shotTrailPrefab);
        return obj;
    }

    void OnTrailGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    void OnTrailRelease(GameObject obj)
    {
        obj.SetActive(false);
    }
}
