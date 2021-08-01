using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelHealth : MonoBehaviour
{
    [SerializeField] private int health;

    public int GetHealth() => health;

    public void SetHealth(int newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
