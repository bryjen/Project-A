using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : EntityBehavior
{
    [Header("Movement Settings")] 
    [SerializeField] private float movementSpeedVelocity;

    [Header("Attack1 Settings")]
    [SerializeField] private float detectionRange, attackCooldown;
    [SerializeField] private int attackDamage;
    [SerializeField] private Vector2 rangeOffsetFromCenter;

    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip attack, death, idle, hit, run;
    
    private IEnumerator standardBehaviorCoroutine;
    private IEnumerator onTimescaleChangedCoroutine;
    
    private void Start()
    {    //todo get rid of this once prefabed // only for testing
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * (1 / Timescale), 0);

        standardBehaviorCoroutine = StandardBehavior();
        onTimescaleChangedCoroutine = OnTimescaleChanged();
        StartCoroutine(standardBehaviorCoroutine);
        StartCoroutine(onTimescaleChangedCoroutine);
        yield break;
    }

    public override IEnumerator StopBehavior()
    {
        StopCoroutine(standardBehaviorCoroutine);
        StopCoroutine(onTimescaleChangedCoroutine);
        yield break;
    }
    
    private IEnumerator StandardBehavior()
    {
        while (true)
        {
            animator.speed = Timescale;
            
            bool areAnyTargetsInRange;
            try
            {
                areAnyTargetsInRange = AreAnyTargetsInRange(detectionRange, rangeOffsetFromCenter);
            }
            catch { yield break; }

            if (areAnyTargetsInRange)
            {
                yield return StartCoroutine(AttackCycle());
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }
            
            if (entityRigidBody.velocity.Equals(Vector2.zero)) 
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * Timescale, 0);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator OnTimescaleChanged()
    {
        var previousTimescale = Timescale;
        
        while (true)
        {
            if (previousTimescale != Timescale && !AreAnyTargetsInRange(detectionRange, rangeOffsetFromCenter))
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * Timescale, 0);

            previousTimescale = Timescale;
            yield return null;
        }
    }

    private IEnumerator AttackCycle()
    {
        animator.Play(attack.name);
        entityRigidBody.velocity = Vector2.zero;

        var damageDelay = .65f;
        yield return new WaitForSeconds(damageDelay * (1 / Timescale));
        try
        {
            DealDamage(GetTargetsInRange(detectionRange, rangeOffsetFromCenter),
                attackDamage);
        }
        catch
        {
            yield break;
        }
        
        yield return new WaitForSeconds((attackCooldown - damageDelay) * (1 / Timescale));
        
        animator.Play(run.name); 
    }
}
