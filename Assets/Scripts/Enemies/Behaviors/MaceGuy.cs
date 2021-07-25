using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceGuy : EnemyBehavior
{
    [Header("Movement Settings"), Space(25)] 
    [SerializeField] private float movementSpeedVelocity;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attack1Damage, cooldownAfterAttack1;
    [SerializeField] private float attack2Damage, cooldownAfterAttack2;
    [SerializeField] private float attack3Damage, cooldownAfterAttack3;

    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip attack1, attack2, attack3, charge1, charge2, death, hit, run;

    private int comboCounter;

    private void Start()
    {    //todo get rid of this once prefabed // only for testing
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        entityRigidBody.velocity = new Vector2(-movementSpeedVelocity, 0);
        comboCounter = 1;
        
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
            if (AreAnyTargetsInRange(attackRange, Vector2.zero))
                yield return StartCoroutine(AttackCycle());

            if (entityRigidBody.velocity.Equals(Vector2.zero)) 
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity, 0);
            animator.Play(run.name);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator AttackCycle()
    {
        animator.Play($"Attack{comboCounter}");
        entityRigidBody.velocity = Vector2.zero;
        
        switch (comboCounter)
        {
            case 1:
                yield return new WaitForSeconds(cooldownAfterAttack1);
                break;
            case 2:
                yield return new WaitForSeconds(cooldownAfterAttack2);
                break;
            case 3:
                yield return new WaitForSeconds(cooldownAfterAttack3);
                break;
            default:
                yield break;
        }
        comboCounter = comboCounter == 3 ? 1 : comboCounter + 1;
    }
}
