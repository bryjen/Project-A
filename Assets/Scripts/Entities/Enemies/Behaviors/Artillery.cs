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
    
    private void Start()
    {    //todo get rid of this once prefabed // only for testing
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * Timescale, 0);

        StartCoroutine(StandardBehavior());
        yield break;
    }

    public override IEnumerator StopBehavior()
    {
        throw new System.NotImplementedException();
    }
    
    private IEnumerator StandardBehavior()
    {
        while (true)
        {
            animator.speed = Timescale;
            if (AreAnyTargetsInRange(detectionRange, rangeOffsetFromCenter))
            {
                yield return StartCoroutine(AttackCycle());
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }
                
            
            if (entityRigidBody.velocity.Equals(Vector2.zero)) 
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * Timescale, 0);
            
            animator.Play(run.name); 
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator AttackCycle()
    {
        animator.Play(attack.name);
        entityRigidBody.velocity = Vector2.zero;

        var damageDelay = .65f;
        yield return new WaitForSeconds(damageDelay * Timescale);
        DealDamage(GetTargetsInRange(detectionRange, rangeOffsetFromCenter),
            attackDamage);
        
        yield return new WaitForSeconds((attackCooldown - damageDelay) * Timescale);
    }
}
