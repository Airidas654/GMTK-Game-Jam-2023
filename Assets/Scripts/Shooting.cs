using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    [SerializeField] GameObject BulletPrefab;
    [SerializeField] float shootingCooldown;
    GameManager game;
    float cooldown;
    void Start()
    {
        game = GameManager.Instance;
        cooldown = 0;
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
            GameObject obj = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (dir.x == 0 && dir.y == 0)
            {
                dir /= Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
            }
            obj.GetComponent<MainBullet>().OnInstance(dir);
        }
    }
}
