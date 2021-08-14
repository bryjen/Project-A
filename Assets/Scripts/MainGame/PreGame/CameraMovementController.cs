using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private float initialDelay;
    [SerializeField] private GameObject cameraGameObject;
    
    [Header("First Movement")]
    [SerializeField] private AnimationCurve firstMovementAnimationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    [SerializeField] private float firstMovementDuration;
    
    [Header("Second Movement")]
    [SerializeField] private AnimationCurve secondMovementAnimationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    [SerializeField] private float secondMovementDuration;
    
    private PreGameController preGameController;

    void Awake()
    {
        preGameController = GetComponent<PreGameController>();
    }

    public IEnumerator GetFirstMovementCoroutine() => 
        MoveCamera(new Vector3(12, 0, -10), firstMovementDuration, firstMovementAnimationCurve);
    
    public IEnumerator GetSecondMovementCoroutine() => 
        MoveCamera(new Vector3(0, 0, -10), secondMovementDuration, secondMovementAnimationCurve);

    public float GetInitialDelay() => initialDelay;

    private IEnumerator MoveCamera(Vector3 destination, float duration, AnimationCurve animationCurve)
    {
        var t = 0f;
        
        do
        {
            t += Time.deltaTime;
            var s = t / duration;

            cameraGameObject.transform.position = Vector3.Lerp(cameraGameObject.transform.position, destination, animationCurve.Evaluate(s));

            if (t > 3)
                break;

            yield return null;
        } while (true);
    }
}
