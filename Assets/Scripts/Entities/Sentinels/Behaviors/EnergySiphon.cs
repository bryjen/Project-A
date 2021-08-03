using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnergySiphon : EntityBehavior
{
    [Header("Prefab Settings")] 
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private Vector2 spawnOffset;
    [SerializeField] private string xVelocityRangeInput, yVelocityRangeInput;

    [Header("Production Settings")] 
    [SerializeField] private float initialDelay, cooldown;
    [SerializeField] private int energyUnitsProduced, energyAmountPerUnit;
    
    [Header("Animator Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip action, idle, hurt, death;

    private Range xVelocityRange, yVelocityRange;
    private bool isInitialized;
    
    private void Awake()
    {
        xVelocityRange = new Range(xVelocityRangeInput);
        yVelocityRange = new Range(yVelocityRangeInput);
        
        var gameData = GameData.Instance;
        gameData.ChangeEnergy(gameData.GetEnergy() - EnergyCost);
        
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        StartCoroutine(DefaultBehavior());
        yield break;
    }

    public override IEnumerator StopBehavior()
    {
        throw new System.NotImplementedException();
    }
    
    private IEnumerator DefaultBehavior()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            for (int i = 0; i < energyUnitsProduced; i++)
            {
                SpawnEnergy();
            }
            
            yield return new WaitForSeconds(cooldown);
        }
    }

    private void SpawnEnergy()
    {
        Vector2 currentPosition = transform.position;
        var spawnedPrefab =
            (GameObject) Instantiate(energyPrefab, currentPosition + spawnOffset, quaternion.identity);
        
        var rigidBody2D = spawnedPrefab.AddComponent<Rigidbody2D>();
        rigidBody2D.velocity = new Vector2(xVelocityRange.GetRandomValue, yVelocityRange.GetRandomValue);
        rigidBody2D.drag = .7f;
        
        var overheadValue = new Range(0, 0.5f);
        spawnedPrefab.AddComponent<ScriptedBounce>()
            .Initialize(transform.position.y + overheadValue.GetRandomValue);

        var energyBehavior = spawnedPrefab.GetComponent<EnergyBehavior>();
        energyBehavior.Initialize(energyAmountPerUnit);
    }
}
