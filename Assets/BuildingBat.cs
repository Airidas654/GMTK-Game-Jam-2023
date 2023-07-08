using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBat : Building
{
    [Space(20)]
    [SerializeField] float enemyDetectionRange = 10f;
    [SerializeField] float enemyHitRange = 1f;
    [SerializeField] float damage = 1f;

    bool hitFlag;
    bool canHit = false;

    public void HitAnimation()
    {
        canHit = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyHitRange);
    }

    private new void Update()
    {
        base.Update();
        if (grown)
        {
            GameObject closest = EnemyManager.Instance.FindClosest(transform.position);
            if (closest == null)
            {
                animator.SetBool(0, false);
                return;
            }

            float distSqr = ((Vector2)closest.transform.position - (Vector2)transform.position).sqrMagnitude;

            if (distSqr <= enemyDetectionRange * enemyDetectionRange)
            {
                if (closest.transform.position.x - transform.position.x < 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }


            }

            if (distSqr <= enemyHitRange * enemyHitRange)
            {
                animator.SetBool(0, true);

                if (canHit)
                {
                    canHit = false;
                    closest.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            else
            {
                canHit = false;
                animator.SetBool(0, false);
            }
        }
    }

}
