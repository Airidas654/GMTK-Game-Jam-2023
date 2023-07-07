using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet : MonoBehaviour
{

    [SerializeField] float Damage;
    [SerializeField] float FlyingSpeed;



    public void OnInstance(Vector2 flyingDirection)
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = flyingDirection * FlyingSpeed;
        if (rigid.velocity.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>().TakeDamage(Damage);
        Destroy(gameObject);
    }


}
