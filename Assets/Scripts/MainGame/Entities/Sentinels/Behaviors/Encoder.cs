using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encoder : EntityBehavior
{
    [Header("Behavior Settings")]
    [SerializeField] private float initialDelay;
    [SerializeField] private float actionTime;

    [Header("Action Settings")]
    [SerializeField] private float attackRange;
    [SerializeField] private bool doesAttackSlow, doesAttackWeaken;    
    //potential status effects

    [Header("Animator Settings")] 
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip teleportIn, teleportOut, action;

    [Header("Other Settings")] 
    [SerializeField] private GameObject temporarySpriteGameObject;

    private void Start()
    {    //todo remove this when done
        StartCoroutine(StartBehavior());
    }
    
    public override IEnumerator StartBehavior()
    {
        Destroy(temporarySpriteGameObject);

        StartCoroutine(StandardBehavior());

        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) //if anim is not playing
            {
                Destroy(this.gameObject);
                
                yield break;
            }
            yield return null;
        }
    }

    public override IEnumerator StopBehavior()
    {
        yield break;
        //Unstoppable Behavior
    }

    private IEnumerator StandardBehavior()
    {
        animator.Play(teleportIn.name);
        yield return new WaitForSeconds(initialDelay);
        
        animator.Play(action.name);
        yield return new WaitForSeconds(.5f);
        DebuffEnemiesInRange(GetTargetsInRange(attackRange, Vector2.zero));
        yield return new WaitForSeconds(actionTime - .5f);
        

        animator.Play(teleportOut.name);
    }

    private void DebuffEnemiesInRange(RaycastHit2D[] targets)
    {
        try
        {
            foreach (var target in targets)
            {
                if (!target.collider.gameObject.TryGetComponent<DebuffHandler>(out DebuffHandler debuffHandler))
                    continue;
                
                debuffHandler.InflictDebuff(Debuff.ENCODED, 5);
            }
        } catch {}
    }
}
