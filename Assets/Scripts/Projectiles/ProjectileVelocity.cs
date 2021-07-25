using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    
    public void Initialize(float velocity)
    {
        _rigidbody2D.velocity = new Vector2(velocity, 0);
    }
}
