using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int maxHealth;
    public int health;

    private NavMeshAgent agent;
    public Transform player;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        health = maxHealth;
    }

    public void FixedUpdate()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
