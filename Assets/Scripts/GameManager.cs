using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int water { get; private set; }
    int imaginaryWater;
    public int MaxWaterInBar;
    public float pumpHealth { get; private set; }
    [SerializeField] float pumpMaxHealth;

    public float RemainingTime { get; private set; }
    [SerializeField] float MaxTime;

    public bool Playing { get; private set; }
    public GameObject Player;
    public Vector2 WorldBorders;

    public List<int> BuildingCosts;

    public List<Sprite> droppletIcons;

    public static GameManager Instance;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(WorldBorders.x * 2, WorldBorders.y * 2));
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        water = 0;
        imaginaryWater = 0;
        pumpHealth = pumpMaxHealth;
        Playing = false;
    }

    Transform camTrans;
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        camTrans = Camera.main.transform;
    }

    public float GetElapsedTime()
    {
        return MaxTime - RemainingTime;
    }

    public bool EnoughWaterForSelectedBuilding()
    {
        return water >= BuildingCosts[UiManager.Instance.selectedBuilding];
    }

    public void AddImaginaryWater(int amount)
    {
        imaginaryWater += amount;
        imaginaryWater = Mathf.Min(imaginaryWater, MaxWaterInBar);
    }

    public void AddWater(int amount)
    {
        water += amount;
        water = Mathf.Min(water, MaxWaterInBar);
        UiManager.Instance.UpdateWaterBar((float)water / MaxWaterInBar);

    }

    public void SubtractWater(int amount)
    {
        water = Mathf.Max(water - amount, 0);
        imaginaryWater = water;
        UiManager.Instance.UpdateWaterBar((float)water / MaxWaterInBar);
    }

    public bool MaxWaterReached()
    {
        return water >= MaxWaterInBar;
    }

    public bool MaxImaginaryWaterReached()
    {
        return imaginaryWater >= MaxWaterInBar;
    }

    bool canShakeCamera = true;
    public void DamagePump(float damage)
    {
        if (Playing)
        {
            SoundManager.Instance.PlayOneShot(2);
            pumpHealth -= damage;
            pumpHealth = Mathf.Max(0, pumpHealth);
            Transform obj = Pump.Instance.gameObject.transform.GetChild(0).GetChild(0);
            DOTween.Kill(obj);
            obj.DOScaleX(pumpHealth / pumpMaxHealth, 0.2f).SetEase(Ease.InOutCubic);

            if (canShakeCamera)
            {
                canShakeCamera = false;
                camTrans.DOShakeRotation(0.1f, new Vector3(0, 0, 2), 1, 10, true, ShakeRandomnessMode.Full).OnComplete(() =>
                {
                    camTrans.DOKill();
                    canShakeCamera = true;
                });
            }

            if (pumpHealth <= 0)
            {
                GameOver();
            }
        }
    }

    public void StartGame()
    {
        canShakeCamera = true;
        RemainingTime = MaxTime;
        Playing = true;
    }

    public void GameOver()
    {
        Playing = false;
        UiManager.survived = (int)MaxTime - Mathf.CeilToInt(RemainingTime);
        UiManager.Instance.ReturnToMeniu();
    }

    private void Update()
    {
        if (Playing && RemainingTime > 0)
        {
            RemainingTime -= Time.deltaTime;
            RemainingTime = Mathf.Max(0, RemainingTime);
            UiManager.Instance.UpdateTimer(Mathf.CeilToInt(RemainingTime));
        }else if (Playing && EnemyManager.Instance.GetAliveEnemyCount() == 0)
        {
            Playing = false;
            UiManager.survived = -1;
            UiManager.Instance.ReturnToMeniu();
        }
    }

}
