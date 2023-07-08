using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int water { get; private set; }
    int imaginaryWater;
    [SerializeField] int MaxWaterInBar;
    public float pumpHealth { get; private set; }
    [SerializeField] float pumpMaxHealth;

    public float PlayingTime { get; private set; }

    public bool Playing { get; private set; }
    public GameObject Player;
    public Vector2 WorldBorders;

    public static GameManager Instance;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(WorldBorders.x*2, WorldBorders.y*2));
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

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
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
