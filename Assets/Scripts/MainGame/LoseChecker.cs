using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseChecker : MonoBehaviour
{
    [SerializeField] private Vector2 rayCastOriginPoint;
    [SerializeField] private float range;
    
    [Header("Debug Modes")]
    [SerializeField] private bool isRaycastsOn;

    private void Update()
    {
        if (IfEnemyPresent())
        {
            GameObject.Find("PostGameManager").GetComponent<OnGameCompleted>().FinishGame(false);
            Destroy(this);
        }
    }

    private bool IfEnemyPresent()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(rayCastOriginPoint, Vector2.down, range, LayerMask.GetMask("Enemies"));

        if (isRaycastsOn)
            Debug.DrawRay(rayCastOriginPoint, Vector3.down * range, 
                raycastHit2D.collider is null ? Color.red : Color.green);

        return raycastHit2D.collider != null;
    }
}
