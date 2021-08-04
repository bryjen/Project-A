using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsDestination : MonoBehaviour
{
    private AnimationCurve animationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    private Vector3 destination;
    private float duration = 1.0f;

    private IEnumerator movementCoroutine;
    
    public void Initialize(AnimationCurve animationCurve, Vector3 destination, float duration)
    {
        if (animationCurve != null)
            this.animationCurve = animationCurve;
        this.destination = destination;
        this.duration = duration;

        movementCoroutine = animationCurve is null ? StandardMovement() : EasingMovement();
        StartCoroutine(movementCoroutine);
    }

    public void StopMovementCoroutine() => StopCoroutine(movementCoroutine);

    private IEnumerator EasingMovement()
    {
        var t = 0f;
        
        do
        {
            t += Time.deltaTime;
            var s = t / duration;

            this.transform.position = Vector3.Lerp(this.transform.position, destination, animationCurve.Evaluate(s));
            
            if (t > duration)
                break;

            yield return null;
        } while (true);
    }

    private IEnumerator StandardMovement()
    {
        var speed = Vector3.Distance(transform.position, destination) / duration;
        
        do
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            yield return null;
        } while (true);
    }
}
