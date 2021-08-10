using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Group : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<int> rowSpawnLocations;
    [SerializeField] private float waitDuration;
    
    private Wave wave;
    private List<GameObject> spawnedEnemies;

    void Start()
    {
        if (enemies.Count != rowSpawnLocations.Count) 
            throw new Exception("Enemies and Row Spawn Locations must be of the same length");

        enemies.ForEach(gameObject =>
        {
            if (!gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                throw new Exception("The gameobject does not contain an EnemyHealth script!");
        });

        wave = transform.parent.GetComponent<Wave>();
    }

    public void StartGroup()
    {
        Spawn();
        StartCoroutine(Checks());
    }

    public void Spawn()
    {
        spawnedEnemies = new List<GameObject>();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            var spawnedEnemy =
                (GameObject) Instantiate(enemies[i], GetSpawnLocation(rowSpawnLocations[i]), Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyHealth>().SetEnemiesList(spawnedEnemies);
            
            spawnedEnemies.Add(spawnedEnemy);
        }
    }

    private IEnumerator Checks()
    {
        var timeElapsed = 0f;

        do
        {
            timeElapsed += Time.deltaTime;
            
            if (timeElapsed >= waitDuration || spawnedEnemies.Count == 0)
                break;

            yield return null;
        } while (true);
        
        wave.StartNextGroup();
    }

    private static Vector3 GetSpawnLocation(int row)
    {
        switch (row)
        {
            default:    //return the first row as default
            case 1:
                return SpawnLocations.row1.transform.position;
            case 2:
                return SpawnLocations.row2.transform.position;
            case 3:
                return SpawnLocations.row3.transform.position;
            case 4:
                return SpawnLocations.row4.transform.position;
            case 5:
                return SpawnLocations.row5.transform.position;
            case -1:
                return GetSpawnLocation(Random.Range(1, 5));
        }
    }
}
