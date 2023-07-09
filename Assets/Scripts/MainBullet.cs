using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        transform.localScale = Vector2.zero;

        transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);

        if (rigid.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        StartCoroutine(Deletion());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (oneTime)
        {
            SoundManager.Instance.PlayOneShot(3);
            if (collision.CompareTag("Enemy"))
            {
                oneTime = false;
                collision.GetComponent<Enemy>().TakeDamage(Damage);
                transform.DOScale(0, 0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
                {
                    Destroy(gameObject);
                });
                
            }
            else if (collision.CompareTag("Building"))
            {
                Building building = collision.GetComponent<Building>();
                if (building.NeedsWater())
                {
                    oneTime = false;
                    building.Replenish();
                    transform.DOScale(0, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        Destroy(gameObject);
                    });
                }


            }
        }
    }

    IEnumerator Deletion()
    {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }

}
