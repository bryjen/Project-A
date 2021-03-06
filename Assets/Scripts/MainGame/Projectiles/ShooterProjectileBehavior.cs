using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterProjectileBehavior : MonoBehaviour
{
    [SerializeField] private float projectileLifespan;
    [SerializeField] private string requiredTag;
    
    private int damage;
    private Animator animator;
    private bool isPiercing;
    private Vector3 newLocalScale;

    private bool hasHit;

    public void Instantiate(int damage, Animator animator, bool isPiercing, Vector3? newLocalScale)
    {
        this.damage = damage;
        this.animator = animator;
        this.isPiercing = isPiercing;

        if (newLocalScale is null)
            this.newLocalScale = new Vector3(2, 2, 1);
        else
            this.newLocalScale = (Vector3) newLocalScale;
    }

    private void Start()
    {
        StartCoroutine(DestroyOnTimeout());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit && !isPiercing)
            return;
        
        if (!other.gameObject.tag.Equals(requiredTag))
            return;

        if (!other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealthScript))
            return;

        hasHit = true;
        Destroy(GetComponent<ProjectileVelocity>());
        
        var rigidBody = GetComponent<Rigidbody2D>();
        if (isPiercing)
            rigidBody.velocity = rigidBody.velocity * .5f;
        else
            rigidBody.velocity = Vector2.zero;
        transform.localScale = new Vector3(2, 2, 1);
        
        enemyHealthScript.SetHealth(
            enemyHealthScript.GetHealth() - damage);
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        animator.Play("Explode");
        
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(this.gameObject);
    }

    private IEnumerator DestroyOnTimeout()
    {
        var timePlaceholder = 0f;

        while (true)
        {
            timePlaceholder += Time.deltaTime;
            if (timePlaceholder >= projectileLifespan)
            {
                Destroy(this.gameObject);
            }

            yield return null;
        }
    }
}
