using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceGuy : EntityBehavior
{
    [Header("Movement Settings"), Space(25)] 
    [SerializeField] private float movementSpeedVelocity;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private int attack1Damage;
    [SerializeField] private float cooldownAfterAttack1;
    [SerializeField] private int attack2Damage;
    [SerializeField] private float cooldownAfterAttack2;
    [SerializeField] private int attack3Damage;
    [SerializeField] private float cooldownAfterAttack3;

    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip attack1, attack2, attack3, charge1, charge2, death, hit, run;

    private IEnumerator standardBehaviorCoroutine;
    private IEnumerator onTimescaleChangedCoroutine;

    private int comboCounter;

    private void Start()
    {    //todo get rid of this once prefabed // only for testing
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        entityRigidBody.velocity = new Vector2(-movementSpeedVelocity, 0);
        comboCounter = 1;

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
                areAnyTargetsInRange = AreAnyTargetsInRange(attackRange, Vector2.zero);
            }
            catch { yield break; }

            if (areAnyTargetsInRange)
            {
                yield return StartCoroutine(AttackCycle());
                continue;
            }
                

            if (entityRigidBody.velocity.Equals(Vector2.zero)) 
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * Timescale, 0);
            
            animator.Play(run.name);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
    private IEnumerator OnTimescaleChanged()
    {
        var previousTimescale = Timescale;
        
        while (true)
        {
            if (previousTimescale != Timescale && !AreAnyTargetsInRange(attackRange, Vector2.zero))
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity * Timescale, 0);

            previousTimescale = Timescale;
            yield return null;
        }
    }

    private IEnumerator AttackCycle()
    {
        animator.Play($"Attack{comboCounter}");
        entityRigidBody.velocity = Vector2.zero;

        switch (comboCounter)
        {
            case 1:
                DealDamage(GetTargetsInRange(attackRange, Vector2.zero), attack1Damage);
                yield return new WaitForSeconds(cooldownAfterAttack1 * (1 / Timescale));
                break;
            case 2:
                DealDamage(GetTargetsInRange(attackRange, Vector2.zero), attack2Damage);
                yield return new WaitForSeconds(cooldownAfterAttack2 * (1 / Timescale));
                break;
            case 3:
                DealDamage(GetTargetsInRange(attackRange, Vector2.zero), (int) (attack1Damage / 1.5));
                yield return new WaitForSeconds((float) (cooldownAfterAttack3 / 3) * (1 / Timescale));
                
                DealDamage(GetTargetsInRange(attackRange, Vector2.zero), (int) (attack2Damage / 1.5));
                yield return new WaitForSeconds((float) (cooldownAfterAttack3 / 1.5) * (1 / Timescale));
                break;
            default:
                yield break;
        }
        comboCounter = comboCounter == 3 ? 1 : comboCounter + 1;
    }
}
