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

    public void Instantiate(int damage, Animator animator)
    {
        this.damage = damage;
        this.animator = animator;
    }

    private void Start()
    {
        StartCoroutine(DestroyOnTimeout());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.tag.Equals(requiredTag))
            return;

        if (!other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealthScript))
            return;

        Destroy(GetComponent<ProjectileVelocity>());
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
