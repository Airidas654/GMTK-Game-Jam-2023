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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitDistance);
    }

    public void Reset()
    {
        stopped = false;
        dead = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            dead = true;
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 pos)
    {
        target = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        //TODO
        //if ((buildingManager.FindClosest(transform.position).position - transform.position).magnitude < hitDistance)
        //{
        //  buildingManager.Hit(damage)
        //}

        if (!stopped)
        {
            Vector2 dir = (target - new Vector2(transform.position.x, transform.position.y)).normalized;
            transform.position += new Vector3(dir.x, dir.y, 0) * Time.deltaTime * movementSpeed;
        }
    }
}
