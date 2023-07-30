using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum StateEnum { Running, Knockback }
    public StateEnum state;
    public bool spawned = false;

    public int maxHealth;
    public int health;
    public float stunnedTimer;

    public Vector3 groundCheckOffset;
    public float groundCheckRadius;
    public LayerMask groundMask;

    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform player;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerStatus>().transform;

        state = StateEnum.Running;
        health = maxHealth;
    }

    public virtual void FixedUpdate()
    {
        if (!spawned) return;

        switch (state) 
        { 
            case StateEnum.Running: 
                RunningBehaviour(); break;
            case StateEnum.Knockback: 
                KnockbackBehaviour(); break;
        }
    }

    public virtual void RunningBehaviour()
    {
        agent.SetDestination(player.position);
    }

    public virtual void KnockbackBehaviour()
    {
        if(CheckGrounded() && stunnedTimer <= 0)
        {
            state = StateEnum.Running;
            agent.enabled = true;
            rb.isKinematic = true;
        }
        else
        {
            stunnedTimer -= Time.deltaTime;
            Mathf.Clamp(stunnedTimer, 0, Mathf.Infinity);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (!spawned) return;

        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }


    public virtual void TakeKnockback(Vector3 force, float time)
    {
        if (!spawned) return;

        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(force);
        stunnedTimer = time;
        state = StateEnum.Knockback;
    }

    public virtual bool CheckGrounded()
    {
        return Physics.CheckSphere(transform.position + groundCheckOffset, groundCheckRadius, groundMask);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
