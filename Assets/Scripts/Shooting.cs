using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    [SerializeField] GameObject BulletPrefab;
    [SerializeField] float shootingCooldown;
    public bool shooting;
    GameManager game;
    float cooldown;
    Animator anim;
    void Start()
    {
        game = GameManager.Instance;
        cooldown = 0;
        shooting = false;
        anim = GetComponent<Animator>();
    }

    Vector2 mousepos;
    public void Shoot()
    {
        GameObject obj = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        Vector2 dir = Camera.main.ScreenToWorldPoint(mousepos) - transform.position;
        if (dir.x == 0 && dir.y == 0)
        {
            dir /= Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
        }
        obj.GetComponent<MainBullet>().OnInstance(dir);
    }

    public void DoneShooting()
    {
        anim.SetBool("Shooting", false);
        shooting = false;
    }

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0) cooldown = 0;
        }
        if (Input.GetMouseButtonDown(0) && cooldown == 0 && game.water != 0)
        {
            cooldown = shootingCooldown;
            game.SubtractWater(1);
            mousepos = Input.mousePosition;
            shooting = true;
            anim.SetBool("Shooting",true);
        }
    }
}
