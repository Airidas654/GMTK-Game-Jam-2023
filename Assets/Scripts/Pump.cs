using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pump : MonoBehaviour
{
    [SerializeField] GameObject dropPrefab;
    ObjectPool<GameObject> drops;

    private void Start()
    {
        drops = new ObjectPool<GameObject>(CreateDrop,GetDrop, ReleaseDrop);
    }

    GameObject CreateDrop()
    {
        GameObject obj = Instantiate(dropPrefab);
        obj.transform.position = gameObject.transform.position;
        obj.GetComponent<PickableDrop>().Reset();
        return obj;
    }

    void GetDrop(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<PickableDrop>().Reset();
    }

    void ReleaseDrop(GameObject obj)
    {
        obj.SetActive(false);
    }
}
