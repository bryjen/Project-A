using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehavior : MonoBehaviour
{
    [Header("Key Components")]
    [SerializeField] protected BoxCollider2D entityBoxCollider;
    [SerializeField] protected Rigidbody2D entityRigidBody;
    [HideInInspector] public float Timescale = 1;
    
    [Header("Debug Settings")]
    [SerializeField] protected bool isRaycastsOn;

    [Header("Raycast Settings")]
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] private bool isDirectedLeft;

    [Header("Sentinel Settings")]
    public int EnergyCost;
    public Vector2 PlacementOffset;

    public abstract IEnumerator StartBehavior();

    public abstract IEnumerator StopBehavior();

    protected bool AreAnyTargetsInRange(float range, Vector2 offsetFromCenter)
    {
        Vector2 boxColliderCenterBounds = entityBoxCollider.bounds.center;
        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(boxColliderCenterBounds + offsetFromCenter, 
            (isDirectedLeft ? Vector2.left : Vector2.right), entityBoxCollider.bounds.extents.x + range, layerMask);

        if (isRaycastsOn)
        {
            var rayColor = raycastHit2D.collider is null ? Color.red : Color.green;
            Debug.DrawRay(boxColliderCenterBounds + offsetFromCenter, 
                (isDirectedLeft ? Vector2.left : Vector2.right) * (entityBoxCollider.bounds.extents.x + range), rayColor);
        }
        
        return raycastHit2D.collider != null;
    }

    protected RaycastHit2D[] GetTargetsInRange(float range, Vector2 offsetFromCenter)
    {
        Vector2 boxColliderCenterBounds = entityBoxCollider.bounds.center;
        
        return Physics2D.RaycastAll(boxColliderCenterBounds + offsetFromCenter, 
            (isDirectedLeft ? Vector2.left : Vector2.right), entityBoxCollider.bounds.extents.x + range, layerMask);
    }
    
    protected void DealDamage(RaycastHit2D[] targets, int damage)
    {
        try
        {
            foreach (var target in targets)
            {
                if (!target.collider.gameObject.TryGetComponent<SentinelHealth>(out SentinelHealth sentinelHealthScript))
                    continue;
            
                sentinelHealthScript.SetHealth(
                    sentinelHealthScript.GetHealth() - damage);
            }
        } catch {}
    }
}
