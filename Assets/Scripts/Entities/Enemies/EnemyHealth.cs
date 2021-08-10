using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    private List<GameObject> enemies;
    
    public override void SetHealth(int newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            Debug.Log(enemies.Contains(this.gameObject));
            enemies.Remove(gameObject);
            
            StartCoroutine(DestroyAllComponents());
            StartCoroutine(AnimationCoroutine());
        }
        
        StartCoroutine(PulseRed());
    }

    public void SetEnemiesList(List<GameObject> enemies) => this.enemies = enemies;

    private IEnumerator DestroyAllComponents()
    {
        if (entityBehaviorScript != null)
        {
            yield return StartCoroutine(entityBehaviorScript.StopBehavior());
            Destroy(entityBehaviorScript);
        }
            
        
        if (boxCollider != null) 
            Destroy(boxCollider);
            
        if (rigidBody != null) 
            Destroy(rigidBody);
    }

    private IEnumerator AnimationCoroutine()
    {
        animator.Play(animationClipName);

        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length + overheadTimeBeforeDespawn);

        StartCoroutine(FadeAway());
    }
}
