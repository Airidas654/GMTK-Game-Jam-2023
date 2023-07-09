using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooting : MonoBehaviour
{

    [SerializeField] GameObject BulletPrefab;
    [SerializeField] float shootingCooldown;
    [SerializeField] int cost;
    public bool shooting;
    bool shotOnce;
    GameManager game;
    float cooldown;
    Animator anim;
    void Start()
    {
        game = GameManager.Instance;
        cooldown = 0;
        shooting = false;
        shotOnce = false;
        anim = GetComponent<Animator>();
    }

    Vector2 mousepos;
    public void Shoot()
    {
        if (!shotOnce)
        {
            SoundManager.Instance.Play(1);
            Vector3 pos = transform.GetChild(0).position;
            GameObject obj = Instantiate(BulletPrefab, pos, Quaternion.identity);
            Vector2 dir = ((Vector2)mousepos - (Vector2)pos).normalized;
            shotOnce = true;
            obj.GetComponent<MainBullet>().OnInstance(dir);
        }
    }

    public void DoneShooting()
    {
        anim.SetBool("Shooting", false);
        shooting = false;

        shotOnce = false;
    }

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0) cooldown = 0;
        }
        if (Input.GetMouseButtonDown(0) && cooldown == 0 && GameManager.Instance.Playing && !UiManager.Instance.inBuildMode && game.water >= cost)
        {
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }
            
            cooldown = shootingCooldown;
            game.SubtractWater(cost);
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousepos.x > transform.position.x)
            {
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            shooting = true;
            anim.SetBool("Shooting",true);
        }
    }
}
