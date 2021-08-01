using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DevEnemySpawner : MonoBehaviour
{
    [Header("Spawning Options")]
    [SerializeField] private Toggle row1Toggle;
    [SerializeField] private Toggle row2Toggle;
    [SerializeField] private Toggle row3Toggle;
    [SerializeField] private Toggle row4Toggle;
    [SerializeField] private Toggle row5Toggle;

    [Space(15)]
    [SerializeField] private GameObject row1SpawnObject;
    [SerializeField] private GameObject row2SpawnObject;
    [SerializeField] private GameObject row3SpawnObject;
    [SerializeField] private GameObject row4SpawnObject;
    [SerializeField] private GameObject row5SpawnObject;

    [Header("Enemy Prefab Options")]
    [SerializeField] private GameObject artilleryPrefab;
    [SerializeField] private GameObject macePrefab;
    [SerializeField] private GameObject mudGuardianPrefab;

    public void SpawnEnemy(string enemyName)
    {
        GameObject enemyToBeSpawned = mudGuardianPrefab;    //mud guardian by default
        switch (enemyName)
        {
            case "artillery" :
                enemyToBeSpawned = artilleryPrefab;
                break;
            case "mace" :
                enemyToBeSpawned = macePrefab;
                break;
            case "guardian" :
                enemyToBeSpawned = mudGuardianPrefab;
                break;
        }

        if (row1Toggle.isOn)
            Instantiate(enemyToBeSpawned, row1SpawnObject.transform.position, quaternion.identity);
        
        if (row2Toggle.isOn)
            Instantiate(enemyToBeSpawned, row2SpawnObject.transform.position, quaternion.identity);
        
        if (row3Toggle.isOn)
            Instantiate(enemyToBeSpawned, row3SpawnObject.transform.position, quaternion.identity);
        
        if (row4Toggle.isOn)
            Instantiate(enemyToBeSpawned, row4SpawnObject.transform.position, quaternion.identity);
        
        if (row5Toggle.isOn)
            Instantiate(enemyToBeSpawned, row5SpawnObject.transform.position, quaternion.identity);
    }
}
