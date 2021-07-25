using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : SentinelBehavior
{
    [Header("Attack Settings")] 
    [SerializeField, Min(1f)] private float initialDelay, attackCooldown;
    [SerializeField] private float attackDetectionRange, damage;
    [SerializeField] private Vector2 rayOffset;


    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip wakeUp, attack, damaged, death;

    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;

    private bool isInitialized;
    
    private void Start()
    {    //todo remove this when done
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        if (!isInitialized)
            yield return StartCoroutine(Initialize());

        StartCoroutine(DefaultBehavior());
    }

    public override IEnumerator StopBehavior()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(.5f);
        
        animator.Play(wakeUp.name);
        
        yield return new WaitForSeconds(initialDelay - .5f);
        spriteRenderer.sprite = idleSprite;
        isInitialized = true;
    }

    private IEnumerator DefaultBehavior()
    {
        while (true)
        {
            //todo check if there is an enemy in the row, if not then wait for time.deltatime

            if (AreAnyTargetsInRange(attackDetectionRange, rayOffset))
            {
                animator.Play(attack.name);
                
                //do something to detect all enemies in the range
                
                yield return new WaitForSeconds(attackCooldown);
                continue;
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
