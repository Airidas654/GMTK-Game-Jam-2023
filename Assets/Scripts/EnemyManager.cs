using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance = null;
    [SerializeField] Vector2 notSpawnBox;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    ObjectPool<GameObject> enemies;

    public void Start()
    {
        enemies = new ObjectPool<GameObject>(CreateEnemy, GetEnemy, ReleaseEnemy);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, notSpawnBox);
    }

    GameObject CreateEnemy()
    {
        GameObject obj = Instantiate(enemyPrefabs[0]);
        obj.GetComponent<Enemy>().Reset();
        return obj;
    }

    void GetEnemy(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<Enemy>().Reset();
    }
    
    void ReleaseEnemy(GameObject obj)
    {
        obj.SetActive(false);
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

    IEnumerator testas()
    {
        SpawnNewEnemy();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(testas());
    }

    public void SpawnNewEnemy()
    {
        GameObject obj = enemies.Get();
        obj.transform.position = GetNewSpawnPosition();

        obj.GetComponent<Enemy>().SetTarget(BuildingManager.Instance.MainBuilding.transform.position);
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
