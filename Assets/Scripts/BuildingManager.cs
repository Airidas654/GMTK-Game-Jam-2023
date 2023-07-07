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
    [SerializeField] Vector2 playableArea;
    [SerializeField] Vector2Int rectangleCount;

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

    public void AddTower(GameObject obj)
    {

    }

    public void DeleteTower(GameObject obj)
    {

    }
    
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

    }
}
