using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBounce : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float groundYValue;

    public void Initialize(float groundYValue)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        this.groundYValue = groundYValue;

        StartCoroutine(BounceCycle());
    }

    private IEnumerator BounceCycle()
    {
        while (true)
        {
            if (_rigidbody2D.velocity.y < 0 && transform.position.y < groundYValue)
            {
                var newYVelocity = -0.8f * _rigidbody2D.velocity.y;
                if (Math.Abs(newYVelocity) < 0.1)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
                    yield break;
                }
                
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, newYVelocity);
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
