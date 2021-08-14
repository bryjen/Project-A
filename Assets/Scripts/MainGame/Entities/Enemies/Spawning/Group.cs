using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Group : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<int> rowSpawnLocations;
    [SerializeField] private float waitDuration;

    private Wave wave;
    private WaveManager waveManager;
    private List<GameObject> spawnedEnemies;

    private int row1SpawnCount = -1;
    private int row2SpawnCount = -1;
    private int row3SpawnCount = -1;
    private int row4SpawnCount = -1;
    private int row5SpawnCount = -1;

    void Awake()
    {
        if (enemies.Count != rowSpawnLocations.Count) 
            throw new Exception("Enemies and Row Spawn Locations must be of the same length");

        enemies.ForEach(gameObject =>
        {
            if (!gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                throw new Exception("The gameobject does not contain an EnemyHealth script!");
        });

        wave = transform.parent.GetComponent<Wave>();
        waveManager = wave.transform.parent.GetComponent<WaveManager>();
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
            //If multiple enemies are spawned in the same row, edit the x value such that they dont appear on top of
            //each other
            var spawnLocation = GetSpawnLocation(rowSpawnLocations[i]);
            spawnLocation.x += GetRowSpawnCount(rowSpawnLocations[i]) * Random.Range(0.1f, 1f);

            var spawnedEnemy =
                (GameObject) Instantiate(enemies[i], spawnLocation, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyHealth>().SetEnemiesList(spawnedEnemies);

            //Fix the sorting order + start playing from a random frame
            spawnedEnemy.GetComponent<SortingGroup>().sortingOrder = 
                rowSpawnLocations[i] == -1 ? GetRowFromPosition(spawnedEnemy.transform.position) * 10 : rowSpawnLocations[i] * 10;

            var animator = spawnedEnemy.transform.GetChild(0).GetComponent<Animator>();
            animator.Play("Run", 0, Random.Range(0f, 1f));

            //Set the parent (only to make it more organized)
            spawnedEnemy.transform.SetParent(SpawnRuntimeObjects.Instance.spawnedEnemyParent.transform);

            spawnedEnemies.Add(spawnedEnemy);
        }
    }

    private IEnumerator Checks()
    {
        var timeElapsed = 0f;

        do
        {
            timeElapsed += Time.deltaTime;

            if (spawnedEnemies.Count == 0 && waveManager.waves.Count == 0 && wave.groups.Count == 0)
            {
                GameObject.Find("PostGameManager").GetComponent<OnGameCompleted>().FinishGame(true);
                yield break;
            }

            if (timeElapsed >= waitDuration || spawnedEnemies.Count == 0)
                break;

            yield return null;
        } while (true);

        wave.StartNextGroup();
    }

    //private void Update() => 
        //Debug.Log($"{SpawnRuntimeObjects.Instance.spawnedEnemyParent.transform.childCount}, , {waveManager.waves.Count}, {wave.groups.Count}");

        private Vector3 GetSpawnLocation(int row)
    {
        switch (row)
        {
            default:    //return the first row as default
            case 1:
                row1SpawnCount++;
                return SpawnLocations.row1.transform.position;
            case 2:
                row2SpawnCount++;
                return SpawnLocations.row2.transform.position;
            case 3:
                row3SpawnCount++;
                return SpawnLocations.row3.transform.position;
            case 4:
                row4SpawnCount++;
                return SpawnLocations.row4.transform.position;
            case 5:
                row5SpawnCount++;
                return SpawnLocations.row5.transform.position;
            case -1:
                return GetSpawnLocation(Random.Range(1, 5));
        }
    }

    private int GetRowFromPosition(Vector3 position)
    {
        if (SpawnLocations.row1.transform.position.y == position.y)
            return 1;
        if (SpawnLocations.row2.transform.position.y == position.y)
            return 2;
        if (SpawnLocations.row3.transform.position.y == position.y)
            return 3;
        if (SpawnLocations.row4.transform.position.y == position.y)
            return 4;
        if (SpawnLocations.row5.transform.position.y == position.y)
            return 5;

        return 1; //default value
    }

    private int GetRowSpawnCount(int row)
    {
        switch (row)
        {
            default:    //default value
            case 1:
                return row1SpawnCount;
            case 2:
                return row2SpawnCount;
            case 3:
                return row3SpawnCount;
            case 4:
                return row4SpawnCount;
            case 5:
                return row5SpawnCount;
        }
    }
}
