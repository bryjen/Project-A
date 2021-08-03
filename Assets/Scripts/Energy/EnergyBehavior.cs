using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBehavior : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float lifespan;

    private int energyValue = 25;    //by default
    private bool isCollectable;

    public void Initialize(int energyValue)
    {
        this.energyValue = energyValue;
        isCollectable = true;
        StartCoroutine(DestroyOnTimeout());
    }

    private IEnumerator DestroyOnTimeout()
    {
        yield return new WaitForSeconds(lifespan);

        isCollectable = false;
        yield return StartCoroutine(FadeTo(new Color(1, 1, 1, 0), .75f));
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (!isCollectable)
            return;
        isCollectable = false;
        
        Destroy(GetComponent<ScriptedBounce>());
        Destroy(GetComponent<Rigidbody2D>());

        var gameData = GameData.Instance;
        gameData.ChangeEnergy(
            gameData.GetEnergy() +  energyValue);

        StartCoroutine(OnMouseDownCoroutine());
    }

    private IEnumerator OnMouseDownCoroutine()
    {
        animator.Play("Collected");
        yield return new WaitForSeconds(
            animator.GetCurrentAnimatorStateInfo(0).length - .5f);
        
        Destroy(this.gameObject);
    }
    
    protected IEnumerator FadeTo(Color newColor, float duration)
    {
        var t = 0f;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}
