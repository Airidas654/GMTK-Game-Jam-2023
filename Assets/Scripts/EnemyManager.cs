using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    static EnemyManager Instance = null;
    static List<GameObject> enemiePrefabs = new List<GameObject>();

    ObjectPool<GameObject> enemies = new ObjectPool<GameObject>(CreateEnemy, GetEnemy, ReleaseEnemy);

    static GameObject CreateEnemy()
    {
        GameObject obj = Instantiate(enemiePrefabs[0]);
        obj.GetComponent<Enemy>().Reset();
        return obj;
    }

    static void GetEnemy(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<Enemy>().Reset();
    }
    
    static void ReleaseEnemy(GameObject obj)
    {
        obj.SetActive(false);
    }

    Vector3 GetNewSpawnPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public void SpawnNewEnemy()
    {
        GameObject obj = enemies.Get();
        obj.transform.position = GetNewSpawnPosition();

        //TODO obj.GetComponent<Enemy>().SetTarget(GameManager.Instance.MainBuilding.transform.position);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
