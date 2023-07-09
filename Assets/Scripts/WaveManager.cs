using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [System.Serializable]
    public struct WaveNode
    {
        public float TimeToStart;
        public int EnemiesCount;
        public float robotDamageMultiplyer;
        public float robotHealthMultiplyer;
    }


    [SerializeField] List<WaveNode> Waves;
    public int currentWave { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        currentWave = 0;
    }

    void Update()
    {
        if(currentWave != Waves.Count && GameManager.Instance.Playing)
        {
            if(GameManager.Instance.GetElapsedTime() >= Waves[currentWave].TimeToStart)
            {
                for(int i = 0;i < Waves[currentWave].EnemiesCount; i++)
                {
                    EnemyManager.Instance.SpawnNewEnemy(Waves[currentWave].robotDamageMultiplyer, Waves[currentWave].robotHealthMultiplyer);
                }
                currentWave++;
            }
        }        
    }
}
