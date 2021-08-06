using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOut : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private string initialDelayRangeInput;
    [SerializeField] private string fadeInDurationRangeInput;
    [SerializeField] private string fadeOutDurationRangeInput;
    [SerializeField] private float cooldown;

    private Range initialDelayRange;
    private Range fadeInDurationRange;
    private Range fadeOutDurationRange;
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        
        initialDelayRange = new Range(initialDelayRangeInput);
        fadeInDurationRange = new Range(fadeInDurationRangeInput);
        fadeOutDurationRange = new Range(fadeOutDurationRangeInput);

        StartCoroutine(WaitInitialDelay());
    }

    IEnumerator WaitInitialDelay()
    {
        yield return new WaitForSeconds(initialDelayRange.GetRandomValue);
        StartCoroutine(
                    FadeAway(new Color(1, 1, 1, 0), fadeInDurationRange.GetRandomValue));
    }

    IEnumerator FadeAway(Color newColor, float duration)
    {
        yield return new WaitForSeconds(cooldown);
        
        var t = 0f;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }

        if (newColor == new Color(1, 1, 1, 1))
            StartCoroutine(FadeAway(new Color(1, 1, 1, 0), fadeInDurationRange.GetRandomValue));
        else 
            StartCoroutine(FadeAway(new Color(1, 1, 1, 1), fadeOutDurationRange.GetRandomValue));
    }
}
