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
        Vector3 pos = transform.GetChild(0).position;
        GameObject obj = Instantiate(BulletPrefab, pos, Quaternion.identity);
        Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(mousepos) - (Vector2)pos).normalized;

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
