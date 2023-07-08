using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float movementSpeed;
    [SerializeField] Vector2 target;
    [SerializeField] float damage;
    [SerializeField] float hitDistance;

    bool stopped = false;
    bool dead = false;

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
        animIndex = Animator.StringToHash("EnemyHit");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    GameObject pump;

    public void Reset()
    {
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

        if (health <= 0 && !dead)
        {
            dead = true;
            EnemyManager.Instance.enemies.Release(gameObject);
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

        if (((Vector2)obj.transform.position - (Vector2)transform.position).sqrMagnitude >= ((Vector2)pump.transform.position - (Vector2)transform.position).sqrMagnitude)
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
