using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private EnemyBehavior enemyBehaviorScript;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Death Settings")] 
    [SerializeField] private float overheadTimeBeforeDespawn;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private string animationClipName;

    public int GetHealth() => health;

    public void SetHealth(int newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            Destroy(enemyBehaviorScript);
            Destroy(boxCollider);
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

    private IEnumerator FadeAway()
    {
        var t = 0f;
        var duration = .15f;

        while (t < 1)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(1, 1, 1, 0), t);

            t += Time.deltaTime / duration;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }

    private IEnumerator PulseRed()
    {
        var t = 0f;
        var duration = .15f;

        while (t < 1)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(1, 0, 0, 1), t);

            t += Time.deltaTime / duration;
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(1, 1, 1, 1), t);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}
