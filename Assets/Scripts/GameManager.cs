using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int water { get; private set; }
    [SerializeField] int MaxWaterInBar;
    public float pumpHealth { get; private set; }
    [SerializeField] float pumpMaxHealth;

    public float PlayingTime { get; private set; }

    public bool Playing { get; private set; }


    public static GameManager Instance;
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
        pumpHealth = pumpMaxHealth;
        Playing = false;
    }

    public void AddWater(int amount)
    {
        water += amount;
        UiManager.Instance.UpdateWaterBar(water / MaxWaterInBar);
    }

    public void SubtractWater(int amount)
    {
        water = Mathf.Max(water - amount, 0);
        UiManager.Instance.UpdateWaterBar(water / MaxWaterInBar);
    }

    public bool MaxWaterReached()
    {
        return water >= MaxWaterInBar;
    }

    public void DamagePump(float damage)
    {
        pumpHealth -= damage;
        if (pumpHealth <= 0)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        PlayingTime = 0;
        Playing = true;
    }

    public void GameOver()
    {
        Playing = false;
    }

    private void Update()
    {
        if (Playing)
        {
            PlayingTime += Time.deltaTime;
        }
    }

}
