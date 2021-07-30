using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SentinelBehavior : MonoBehaviour
{
    [SerializeField] protected int EnergyCost;
    
    [Header("Key Components")]
    [SerializeField] protected BoxCollider2D entityBoxCollider;

    [Header("Debug Settings")]
    [SerializeField] protected bool isRaycastsOn;

    [Header("Raycast Settings")]
    [SerializeField] protected LayerMask EnemyLayerMask;

    public abstract IEnumerator StartBehavior();

    public abstract IEnumerator StopBehavior();

    public int GetEnergyCost() => EnergyCost;
    
    protected bool AreAnyTargetsInRange(float range, Vector2 offsetFromCenter)
    {
        Vector2 boxColliderCenterBounds = entityBoxCollider.bounds.center;
        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxColliderCenterBounds + offsetFromCenter, 
            Vector2.right, entityBoxCollider.bounds.extents.x + range, EnemyLayerMask);

        if (isRaycastsOn)
        {
            var rayColor = raycastHit2D.collider is null ? Color.red : Color.green;
            Debug.DrawRay(boxColliderCenterBounds + offsetFromCenter, 
                Vector2.right * (entityBoxCollider.bounds.extents.x + range), rayColor);
        }
        
        return raycastHit2D.collider != null;
    }
}
