using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Shooter : EntityBehavior
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float velocity;
    [SerializeField] private Vector2 spawnOffset;
    [SerializeField] private int damage;
    
    [Header("Behavior Settings")]
    
    [SerializeField, Min(1f)] private float initialDelay;
    [SerializeField, Min(1)] private int projectilesPerShot;
    [SerializeField, Min(0)] private float cooldownBetweenProjectiles, cooldownBetweenShots;
    //cooldownBetweenProjectiles <=> The time delay after each projectile in the same shot cycle
    
    [Space(20)]
    [SerializeField] private bool HasChargeUpAbility;
    [SerializeField] private float ChargeUpTime;

    [Header("Animator Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip idle1, wakeUpAnimation, shootWithFx, shoot, charge, damaged;

    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;

    private bool isInitialized;

    private void Start()
    {    //todo remove this when done
        //StartCoroutine(StartBehavior());
    }
    

    public override IEnumerator StartBehavior()
    {
        if (!isInitialized)
        {
            var gameData = GameData.Instance;
            gameData.ChangeEnergy(gameData.GetEnergy() - (int) EnergyCost);
            
            yield return StartCoroutine(Initialize());
        }
            
        
        StartCoroutine(DefaultBehavior());
    }

    public override IEnumerator StopBehavior()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(.5f);
        
        animator.Play(wakeUpAnimation.name);
        
        yield return new WaitForSeconds(initialDelay - 0.5f);
        spriteRenderer.sprite = idleSprite;
        isInitialized = true;
    }
    
    private IEnumerator DefaultBehavior()
    {
        while (true)
        {
            //todo check if there is an enemy in the row, if not then wait for time.deltatime
            
            for (var i = 0; i < projectilesPerShot; i++)
            {
                animator.Play(shootWithFx.name);
                
                SpawnProjectile();
                if (i != projectilesPerShot - 1)
                    yield return new WaitForSeconds(cooldownBetweenProjectiles);
            }
            
            yield return new WaitForSeconds(cooldownBetweenShots);
        }
    }

    private void SpawnProjectile()
    {
        var spawnLocation = transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0);
        var shooterProjectile = (GameObject) Instantiate(projectilePrefab, spawnLocation, Quaternion.identity);
        
        shooterProjectile.GetComponent<ProjectileVelocity>().Initialize(velocity);
        
        var projectileBehavior = shooterProjectile.GetComponent<ShooterProjectileBehavior>();
        projectileBehavior.Instantiate(damage, shooterProjectile.GetComponent<Animator>());
    }
}