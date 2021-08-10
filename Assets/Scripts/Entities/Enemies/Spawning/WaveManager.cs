using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] private GameObject waveText;

    public List<Wave> GetWaves() => waves;

    public GameObject GetWaveText() => waveText;
    
    public void StartNextWave()
    {
        if (waves.Count == 0)
            return;

        waves[0].StartNextGroup();
        waves.RemoveAt(0);
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
 