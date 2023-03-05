using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth;
    public int health;

    private void Awake()
    {
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if(health == 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Mathf.Clamp(health, 0, maxHealth);
    }

    public void HealDamage(int heal)
    {
        health += heal;
        Mathf.Clamp(health, 0, maxHealth);
    }

    public void Die()
    {

    }
}
