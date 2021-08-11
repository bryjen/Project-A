using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EntityBehavior enemyBehavior;

    public EntityBehavior GetEntityBehavior() => enemyBehavior;
    
    private void Start()
    {
        
    }
}
