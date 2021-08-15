using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject waveText;
    [SerializeField] private TextMeshProUGUI waveCounter;

    private int currentWave;
    private int totalWaves;

    public List<Wave> waves { get; private set; }

    void Awake()
    {
        waves = new List<Wave>();

        foreach (Transform child in transform)
        {
            if (!child.gameObject.TryGetComponent<Wave>(out Wave wave))
                throw new Exception($"The gameobject {child.name} does not contain a Wave script!");
            
            waves.Add(wave);
        }

        totalWaves = waves.Count;
        currentWave = 0;
    }

    public GameObject GetWaveText() => waveText;
    
    public void StartNextWave()
    {
        if (waves.Count == 0)
            return;

        waves[0].StartNextGroup();
        waves.RemoveAt(0);

        if (!waveCounter.gameObject.activeSelf)
            waveCounter.gameObject.SetActive(true);
        
        currentWave++;
        waveCounter.text = $"Waves: {currentWave}/{totalWaves}";
    }
}

public class SpawnLocations
{
    public static readonly GameObject row1;
    public static readonly GameObject row2;
    public static readonly GameObject row3;
    public static readonly GameObject row4;
    public static readonly GameObject row5;

    static SpawnLocations()
    {
        row1 = GameObject.Find("1");
        row2 = GameObject.Find("2");
        row3 = GameObject.Find("3");
        row4 = GameObject.Find("4");
        row5 = GameObject.Find("5");
    }
}
 