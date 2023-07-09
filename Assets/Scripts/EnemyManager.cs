using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance = null;
    [SerializeField] Vector2 notSpawnBox;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    public ObjectPool<GameObject> enemies;

    HashSet<GameObject> aliveEnemies = new HashSet<GameObject>();

    [SerializeField] float DefaultRobotDamage;
    [SerializeField] float DefaultRobotHealth;

    public int GetAliveEnemyCount()
    {
        return aliveEnemies.Count;
    }

    public void Start()
    {
        enemies = new ObjectPool<GameObject>(CreateEnemy, GetEnemy, ReleaseEnemy);
        //StartCoroutine(testas());
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, notSpawnBox);
    }

    GameObject CreateEnemy()
    {
        int rand = Random.Range(0,enemyPrefabs.Count);

        GameObject obj = Instantiate(enemyPrefabs[rand]);
        return obj;
    }

    void GetEnemy(GameObject obj)
    {
        obj.SetActive(true);
        
    }
    
    void ReleaseEnemy(GameObject obj)
    {
        aliveEnemies.Remove(obj);
        obj.SetActive(false);
        obj.transform.position = new Vector2(100, 0);
    }

    public GameObject FindClosest(Vector2 pos)
    {
        float dist = Mathf.Infinity;
        GameObject ans = null;

        foreach (var tow in aliveEnemies)
        {
            if (tow != null)
            {
                Vector2 temp = tow.transform.position;
                float sqrtMag = (temp - pos).sqrMagnitude;
                if (sqrtMag < dist)
                {
                    dist = sqrtMag;
                    ans = tow;
                }
            }
        }

        return ans;
    }

    Vector3 GetNewSpawnPosition()
    {
        Vector3 ans = new Vector3(Random.Range(-notSpawnBox.x/2,notSpawnBox.x/2), Random.Range(-notSpawnBox.y / 2, notSpawnBox.y / 2),0);
        int rand = Random.Range(0,4);
        switch (rand)
        {
            case 0:
                ans = new Vector3(-notSpawnBox.x / 2, ans.y,0);
                break;
            case 1:
                ans = new Vector3(notSpawnBox.x / 2, ans.y, 0);
                break;
            case 2:
                ans = new Vector3(ans.x, -notSpawnBox.y / 2, 0);
                break;
            default:
                ans = new Vector3(ans.x, notSpawnBox.y / 2, 0);
                break;
        }
        return ans;
    }

   

    public void SpawnNewEnemy(float damageMultiplyer, float healthMultiplyer)
    {
        GameObject obj = enemies.Get();
        obj.transform.position = GetNewSpawnPosition();
        aliveEnemies.Add(obj);
        Enemy e = obj.GetComponent<Enemy>();
        e.MaxHealth = DefaultRobotHealth * healthMultiplyer;
        e.damage = DefaultRobotDamage * damageMultiplyer;
        e.Reset();
        e.SetTarget(BuildingManager.Instance.MainBuilding.transform.position);
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
