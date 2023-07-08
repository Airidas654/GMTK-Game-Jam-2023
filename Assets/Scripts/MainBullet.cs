using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet : MonoBehaviour
{

    [SerializeField] float Damage;
    [SerializeField] float FlyingSpeed;

    bool oneTime;

    public void OnInstance(Vector2 flyingDirection)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        oneTime = true;
        rigid.velocity = flyingDirection * FlyingSpeed;
        if (rigid.velocity.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (oneTime)
        {
            if (collision.CompareTag("Enemy"))
            {
                oneTime = false;
                collision.GetComponent<Enemy>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Building"))
            {
                Building building = collision.GetComponent<Building>();
                if (building.NeedsWater())
                {
                    oneTime = false;
                    building.Replenish();
                    Destroy(gameObject);
                }
                

            }
        }
    }


}
