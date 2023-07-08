using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] float health;
    bool dead = false;

    float waterVal = 1;
    [SerializeField] float waterDepletionTimeInSec = 60f;
    [SerializeField] float replenishVal = 0.5f;

    protected bool working = true;

    public bool NeedsWater()
    {
        return waterVal <= 0.75f;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            dead = true;
            working = false;
            BuildingManager.Instance.DeleteTower(gameObject);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public virtual void Replenish()
    {
        waterVal += replenishVal;
        waterVal = Mathf.Min(waterVal, 1);

        if (waterVal == 0)
        {
            working = false;
        }
        else
        {
            working = true;
        }
    }

    
    void Update()
    {
        waterVal -= 1 / waterDepletionTimeInSec * Time.deltaTime;
        waterVal = Mathf.Max(0, waterVal);

        if (waterVal == 0)
        {
            working = false;
        }
        else
        {
            working = true;
        }
    }
}
