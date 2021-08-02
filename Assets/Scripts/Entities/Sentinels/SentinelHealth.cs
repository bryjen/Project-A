using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelHealth : Health
{
    public override void SetHealth(int newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            if (entityBehaviorScript != null) 
                Destroy(entityBehaviorScript);
            
            if (boxCollider != null) 
                Destroy(boxCollider);
            
            if (rigidBody != null) 
                Destroy(rigidBody);
            
            StartCoroutine(AnimationCoroutine());
        }

        StartCoroutine(PulseRed());
    }

    private IEnumerator AnimationCoroutine()
    {
        animator.Play(animationClipName);
        
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length + overheadTimeBeforeDespawn);

        StartCoroutine(FadeAway());
    }
}
