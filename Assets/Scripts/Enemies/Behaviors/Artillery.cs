using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : EnemyBehavior
{
    [Header("Movement Settings")] 
    [SerializeField] private float movementSpeedVelocity;

    [Header("Attack1 Settings")]
    [SerializeField] private float detectionRange;
    [SerializeField] private Vector2 rangeOffsetFromCenter;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown;
    [Space(10)] 
    [SerializeField] private float attackRange;
    [SerializeField] private Vector2 attackRangeOffsetFromCenter;

    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip attack;
    [SerializeField] private AnimationClip death;
    [SerializeField] private AnimationClip idle;
    [SerializeField] private AnimationClip hit;
    [SerializeField] private AnimationClip run;
    
    private void Start()
    {    //todo get rid of this once prefabed // only for testing
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        entityRigidBody.velocity = new Vector2(-movementSpeedVelocity, 0);

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
            if (AreAnyTargetsInRange(detectionRange, rangeOffsetFromCenter))
            {
                yield return StartCoroutine(AttackCycle());
                yield return new WaitForSeconds(Time.deltaTime);
                continue;
            }
                
            
            if (entityRigidBody.velocity.Equals(Vector2.zero)) 
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity, 0);
            animator.Play(run.name); 
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator AttackCycle()
    {
        animator.Play(attack.name);
        entityRigidBody.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(attackCooldown);
    }
}
