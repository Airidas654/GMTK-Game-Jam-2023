using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [System.Serializable]
    class NoBuildZone
    {
        public Vector2 size;
        public Vector2 position;
    }

    public static BuildingManager Instance = null;

    [SerializeField] Vector2 playableArea;
    [SerializeField] Vector2Int rectangleCount;
    Vector2 step;

    [SerializeField] List<NoBuildZone> noBuildZones = new List<NoBuildZone>();

    [Space(20)]

    [SerializeField] Material cursorObjectMaterial;
    [SerializeField] GameObject cursorObject;
    [SerializeField] List<Sprite> buildingCursorSprites = new List<Sprite>();
    [SerializeField] List<GameObject> buildingPrefabs = new List<GameObject>();

    [Space(20)]
    public GameObject MainBuilding;
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
        BuildMode = false;
        SelectedBuilding = 0;
    }

    Dictionary<GameObject, Vector2Int> towers = new Dictionary<GameObject, Vector2Int>();

    Vector2Int GetIndices(Vector2 position)
    {
        Vector2 newPos = position + new Vector2(playableArea.x / 2, playableArea.y / 2);
        newPos = Vector2.Max(Vector2.zero, Vector2.Min(newPos, playableArea));

        return new Vector2Int(Mathf.FloorToInt(newPos.x / step.x), Mathf.FloorToInt(newPos.y / step.y));
    }

    public GameObject FindClosest(Vector2 pos)
    {
        float dist = Mathf.Infinity;
        GameObject ans = null;

        foreach(var tow in towers)
        {
            if (tow.Key != null)
            {
                Vector2 temp = tow.Key.transform.position;
                float sqrtMag = (temp - pos).sqrMagnitude;
                if (sqrtMag < dist)
                {
                    dist = sqrtMag;
                    ans = tow.Key;
                }
            }
        }

        return ans;
    }

    public void AddTower(GameObject obj)
    {
        Vector2Int index = GetIndices(obj.transform.position);
        occupied[index.y, index.x] = true;
        towers.Add(obj,index);
    }

    public void DeleteTower(GameObject obj)
    {
        if(towers.ContainsKey(obj)){
            Vector2Int index = towers[obj];
            occupied[index.y, index.x] = false;

            towers.Remove(obj);
        }
    }

    bool CanPlace(Vector3 pos)
    {
        Vector2Int indices = GetIndices(pos);
        return !occupied[indices.y, indices.x];
    }

    bool buildMode;
    int selectedBuilding;

    public bool BuildMode {
        get
        {
            return buildMode;
        }
        
        set 
        {
            buildMode = value;
            cursorObject.transform.DOKill();

            float tweenVal = buildMode ? 1 : 0;

            if (buildMode)
            {
                cursorObject.SetActive(true);
            }

            cursorObject.transform.DOScale(tweenVal, 0.5f).SetEase(Ease.InOutCirc).OnComplete(() =>
            {
                cursorObject.SetActive(value);
            });
        } 
    }

    public int SelectedBuilding
    {
        get
        {
            return selectedBuilding;
        }
        set
        {
            selectedBuilding = Mathf.Max(0,Mathf.Min(value, buildingCursorSprites.Count-1));
            cursorObject.GetComponent<SpriteRenderer>().sprite = buildingCursorSprites[selectedBuilding];
        }
    }

    void Update()
    {
        if (buildMode)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            cursorObject.transform.position = GetIndices(mousePos) * step - playableArea / 2 + step / 2;

            if (CanPlace(mousePos))
            {
                cursorObjectMaterial.color = Color.white;
                if (Input.GetKeyDown(KeyCode.Mouse0) && !(EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null))
                {
                    if (GameManager.Instance.EnoughWaterForSelectedBuilding())
                    {
                        GameManager.Instance.SubtractWater(GameManager.Instance.BuildingCosts[selectedBuilding]);
                        GameObject obj = Instantiate(buildingPrefabs[selectedBuilding], cursorObject.transform.position, Quaternion.identity);
                        obj.transform.localScale = Vector3.zero;
                        obj.transform.DOScale(1, 1).SetEase(Ease.OutBounce);
                        AddTower(obj);
                    }
                }
            }
            else
            {
                cursorObjectMaterial.color = new Color(1, 74 / 255f, 74 / 255f);
            }
        }
    }
}
