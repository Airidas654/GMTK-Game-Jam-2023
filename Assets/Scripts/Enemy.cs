using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    [SerializeField] float movementSpeed;
    [SerializeField] Vector2 target;
    [SerializeField] public float damage;
    [SerializeField] float hitDistance;

    float health;

    bool stopped = false;
    protected bool dead = false;

    SpriteRenderer spriteRenderer;
    Animator animator;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitDistance);
    }

    int animIndex;

    private void Awake()
    {
        health = MaxHealth;
        animIndex = Animator.StringToHash("EnemyHit");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    GameObject pump;

    Color pradCol, hitCol;

    public void Reset()
    {
        health = MaxHealth;

        pradCol = spriteRenderer.color;
        pradCol.a = 0;
        hitCol = new Color(pradCol.r, pradCol.g, pradCol.b, 1);

        spriteRenderer.color = pradCol;

        stopped = false;
        dead = false;
        pump = Pump.Instance.gameObject;
    }

    bool canHit = false;
    public void AnimationHit()
    {
        canHit = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        spriteRenderer.DOKill();
        spriteRenderer.DOColor(hitCol, 0.1f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            spriteRenderer.DOColor(pradCol, 0.1f).SetEase(Ease.OutQuart);
        });

        if (health <= 0 && !dead)
        {
            dead = true;
            animator.SetBool(animIndex, false);
            EnemyManager.Instance.enemies.Release(gameObject);
            spriteRenderer.DOKill();
            //Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 pos)
    {
        if (pos.x - transform.position.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        target = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;


        GameObject obj = BuildingManager.Instance.FindClosest(transform.position);
        if (obj == null || (((Vector2)obj.transform.position - (Vector2)transform.position).sqrMagnitude >= ((Vector2)pump.transform.position - (Vector2)transform.position).sqrMagnitude))
        {
            obj = pump;
        }

        if (obj != null && ((Vector2)obj.transform.position - (Vector2)transform.position).sqrMagnitude < hitDistance*hitDistance)
        {
            //buildingManager.Hit(damage)
            animator.SetBool(animIndex, true);
            stopped = true;

           
            if (canHit)
            {
                canHit = false;
                if (obj == pump)
                {
                    GameManager.Instance.DamagePump(damage);
                }
                else
                {
                    obj.GetComponent<Building>().TakeDamage(damage);
                }
            }
        }
        else
        {
            animator.SetBool(animIndex, false);
            canHit = false;
            stopped = false;
        }

        if (!stopped)
        {
            Vector2 dir = (target - new Vector2(transform.position.x, transform.position.y)).normalized;
            transform.position += new Vector3(dir.x, dir.y, 0) * Time.deltaTime * movementSpeed;
        }
    }
}
