using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Header("Death Settings")] 
    [SerializeField] protected float overheadTimeBeforeDespawn;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected string animationClipName;
    
    [Header("General Settings")]
    [SerializeField] protected EntityBehavior entityBehaviorScript;
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected Rigidbody2D rigidBody;
    [SerializeField] protected int health;
    
    private Color defaultColor = Color.white;

    public abstract void SetHealth(int newHealth);
    
    public int GetHealth() => health;

    public void SetDefaultColor(Color color)
    {
        defaultColor = color;
        spriteRenderer.color = defaultColor;
    }
    
    protected IEnumerator FadeAway()
    {
        var t = 0f;
        var duration = .15f;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(1, 1, 1, 0), t);

            t += Time.deltaTime / duration;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }

    protected IEnumerator PulseRed()
    {
        var t = 0f;
        var duration = .15f;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(1, 0, 0, 1), t);

            t += Time.deltaTime / duration;
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, defaultColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}
