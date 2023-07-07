using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [System.Serializable]
    class NoBuildZone
    {
        public Vector2 size;
        public Vector2 position;
    }

    static BuildingManager Instance = null;

    [SerializeField] Vector2 playableArea;
    [SerializeField] Vector2Int rectangleCount;
    Vector2 step;

    [SerializeField] List<NoBuildZone> noBuildZones = new List<NoBuildZone>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < noBuildZones.Count; i++)
        {
            Gizmos.DrawWireCube(noBuildZones[i].position, noBuildZones[i].size);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, playableArea);
    }

    bool[,] occupied;

    void Start()
    {
        occupied = new bool[rectangleCount.y, rectangleCount.x];

        step = playableArea/rectangleCount;

        for (int i = 0; i < rectangleCount.y; i++)
        {
            for (int j = 0; j < rectangleCount.x; j++)
            {
                occupied[i, j] = false;
            }
        }

        for (int l = 0; l < noBuildZones.Count; l++)
        {
            int nuoY, ikiY, nuoX, ikiX;
            nuoY = Mathf.FloorToInt(noBuildZones[l].position.y - noBuildZones[l].size.y / 2);
            nuoX = Mathf.FloorToInt(noBuildZones[l].position.x - noBuildZones[l].size.x / 2);

            ikiY = Mathf.CeilToInt(noBuildZones[l].position.y + noBuildZones[l].size.y / 2);
            ikiX = Mathf.CeilToInt(noBuildZones[l].position.x + noBuildZones[l].size.x / 2);

            for (int i = Mathf.Max(nuoY,0); i <= Mathf.Min(ikiY, rectangleCount.y-1); i++)
            {
                for (int j = Mathf.Max(nuoX,0); j <= Mathf.Min(ikiX, rectangleCount.x - 1); j++)
                {
                    occupied[i, j] = true;
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Vector2Int GetIndices(Vector2 position)
    {
        Vector2 newPos = position + new Vector2(playableArea.x / 2, playableArea.y / 2);
        newPos = Vector3.Max(Vector3.zero, Vector3.Min(newPos, playableArea));

        return new Vector2Int(Mathf.FloorToInt(newPos.x / step.x), Mathf.FloorToInt(newPos.y / step.y));
    }

    public void AddTower(GameObject obj)
    {
        Vector2Int index = GetIndices(obj.transform.position);
        occupied[index.y, index.x] = true;
    }

    public void DeleteTower(GameObject obj)
    {
        Vector2Int index = GetIndices(obj.transform.position);
        occupied[index.y, index.x] = false;
    }
    
    void Update()
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.DrawRay(GetIndices(mousePos)*step-playableArea/2, Vector3.up, Color.cyan, 0.1f);
    }
}
