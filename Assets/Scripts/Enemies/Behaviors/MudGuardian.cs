using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudGuardian : EnemyBehavior
{
    [Header("Movement Settings")] 
    [SerializeField] private float movementSpeedVelocity;

    [Header("Attack1 Settings")] 
    [SerializeField] private float attack1Range, attack1Damage;

    [Header("Attack2 Settings")] 
    [SerializeField] private bool isSpecialAttackEnabled;
    [SerializeField] private float attack2Range, attack2Damage;
    [SerializeField] private int triggersAfterHowManyAttacks;    //The amount of times Attack1 that happens before Attack2


    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip attack1, attack2, death, hit, run;

    private int attack1Counter;

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
            if (AreAnyTargetsInRange(attack1Range, Vector2.zero))
                yield return StartCoroutine(AttackCycle());
            
            if (entityRigidBody.velocity.Equals(Vector2.zero)) 
                entityRigidBody.velocity = new Vector2(-movementSpeedVelocity, 0);
            animator.Play(run.name);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator AttackCycle()
    {
        if (attack1Counter % triggersAfterHowManyAttacks == 0 && attack1Counter != 0)
        {
            animator.Play(attack2.name);
            attack1Counter = 0;
        }
        else
        {
            animator.Play(attack1.name);
            attack1Counter++;
        }
        entityRigidBody.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(.75f);
        yield break;
    }
}
