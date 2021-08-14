using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : EntityBehavior
{
    [Header("Attack Settings")] 
    [SerializeField, Min(1f)] private float initialDelay, attackCooldown;
    [SerializeField] private float attackDetectionRange;
    [SerializeField] private int damage;
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
            if (AreAnyTargetsInRange(attackDetectionRange, rayOffset))
            {
                yield return StartCoroutine(AttackCycle());
                continue;
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator AttackCycle()
    {
        animator.Play(attack.name);

        var cooldownBeforeAttack = .25f;
        yield return new WaitForSeconds(cooldownBeforeAttack);
        
        DealDamage(GetTargetsInRange(attackDetectionRange, rayOffset), damage);
        //Debug.Log(GetTargetsInRange(attackDetectionRange, rayOffset).Length);
                
        yield return new WaitForSeconds(attackCooldown - cooldownBeforeAttack);
    }
}
