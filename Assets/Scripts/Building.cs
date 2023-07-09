using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] float health;
    bool dead = false;

    protected float waterVal = 1;
    [SerializeField] float waterDepletionTimeInSec = 60f;
    [SerializeField] float replenishVal = 0.5f;

    protected bool working = true;
    protected bool grown = false;

    [Space(20)]
    [SerializeField] List<Sprite> growthSprites = new List<Sprite>();
    [SerializeField] float growthTime = 20f;

    List<Sprite> droppletIcons;
    SpriteRenderer droppletVisual;

    float growthVal;
    float step;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    public void Start()
    {
        droppletIcons = GameManager.Instance.droppletIcons;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        droppletVisual = transform.GetChild(1).GetComponent<SpriteRenderer>();

        bool randFlip = Random.Range(0, 2) == 0 ? true : false;
        spriteRenderer.flipX = randFlip;

        growthVal = 0;
        step = growthTime / (growthSprites.Count - 1);
    }

    public bool NeedsWater()
    {
        return waterVal <= 0.5f;
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

    void TryChangeDropIcon()
    {
        if (NeedsWater())
        {
            droppletVisual.gameObject.SetActive(true);

            droppletVisual.sprite = droppletIcons[waterVal > 0 ? 0 : 1];
        }
        else
        {
            droppletVisual.gameObject.SetActive(false);
        }
    }

    public virtual void Replenish()
    {
        waterVal += replenishVal;
        waterVal = Mathf.Min(waterVal, 1);

        working = true;
        TryChangeDropIcon();
    }

    
    public void Update()
    {
        if (grown)
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
            TryChangeDropIcon();
        }
        else
        {
            growthVal += Time.deltaTime;

            spriteRenderer.sprite = growthSprites[Mathf.Max(0, Mathf.Min(Mathf.FloorToInt(growthVal/step), growthSprites.Count-1))];

            if (growthVal >= growthTime)
            {
                spriteRenderer.sprite = growthSprites[growthSprites.Count - 1];
                animator.enabled = true;
                grown = true;
            }
        }
    }
}
