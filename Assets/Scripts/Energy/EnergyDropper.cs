using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDropper : MonoBehaviour
{
    [Header("Spawning Behavior")]
    [SerializeField] private float initialDelay;
    [SerializeField] private string cooldownRangeInput;

    [Space(10)] 
    
    [SerializeField] private string xAxisSpawnRangeInput;
    [SerializeField] private string yAxisDestinationRangeInput;

    [Header("Energy Settings")] 
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private int energyValue;
    [SerializeField] private float lifeSpan = 7;

    private Range cooldownRange;
    private Range xAsisSpawnRange;
    private Range yAxisSpawnRange;
    private IEnumerator defaultCoroutine;

    private void Awake()
    {
        cooldownRange = new Range(cooldownRangeInput);
        xAsisSpawnRange = new Range(xAxisSpawnRangeInput);
        yAxisSpawnRange = new Range(yAxisDestinationRangeInput);

        defaultCoroutine = DefaultCoroutine();
        StartCoroutine(defaultCoroutine);
    }

    public void StopCoroutines()
    {
        
    }

    private IEnumerator DefaultCoroutine()
    {
        yield return new WaitForSeconds(initialDelay);
        
        do
        {
           InstantiateEnergyPrefab();
            
            yield return new WaitForSeconds(cooldownRange.GetRandomValue);
        } while (true);
    }

    private void InstantiateEnergyPrefab()
    {
         var spawnPositon = new Vector2(xAsisSpawnRange.GetRandomValue, 7);
         var destination = new Vector2(spawnPositon.x, yAxisSpawnRange.GetRandomValue);
         var spawnedEnergy = (GameObject) Instantiate(energyPrefab, spawnPositon, Quaternion.identity);
         
         spawnedEnergy.AddComponent<MoveTowardsDestination>().Initialize(null, destination, 5);
         spawnedEnergy.GetComponent<EnergyBehavior>().Initialize(energyValue, lifeSpan);
    }
}
