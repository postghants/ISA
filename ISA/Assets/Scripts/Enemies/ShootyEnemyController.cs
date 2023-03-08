using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootyEnemyController : MonoBehaviour
{
    public enum StateEnum { Standing, Shooting, Running }
    public StateEnum state;

    private NavMeshAgent agent;
    public Transform player;

    [Header("Health")]
    public int maxHealth = 10;
    public int health;

    [Header("Shoot")]
    public GameObject shootProjectilePrefab;
    public Transform shootOffset;
    public int shootDamage;
    public float shootSpeed;
    public float shootLifespan;
    public float shootStartup;
    public float shootWalkCooldown;
    public float shootCooldown;
    [HideInInspector] public bool shootAllowed = true;

    [Header("Run Away")]
    public float runAwayDistance = 4f;
    public float lenghtOfRun = 5f;
    public float runAwayCooldown = 3f;
    [HideInInspector] public bool runAwayAllowed = true;


    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
        state = StateEnum.Standing;
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case StateEnum.Standing:
                StandingBehaviour(); break;
            case StateEnum.Running:
                break;
            case StateEnum.Shooting:
                break;

        }
    }

    public void StandingBehaviour()
    {
        transform.rotation = Quaternion.Euler(0, player.transform.rotation.y - 180, 0);

        if (Vector3.Distance(transform.position, player.position) < runAwayDistance && runAwayAllowed)
        {
            RunAway(player.position);
        }
        else if (shootAllowed)
        {
            StartCoroutine(Shoot());
        }
    }

    public IEnumerator Shoot()
    {
        state = StateEnum.Shooting;
        shootAllowed = false;
        yield return new WaitForSeconds(shootStartup);
        GameObject projectile = Instantiate(shootProjectilePrefab);
        ProjectileController pc = projectile.GetComponent<ProjectileController>();
        projectile.transform.position = shootOffset.position;
        pc.shooter = this.gameObject;
        pc.damage = shootDamage;
        pc.speed = shootSpeed;
        pc.lifespan = shootLifespan;
        pc.direction = player.position - transform.position;
        yield return new WaitForSeconds(shootWalkCooldown);
        state = StateEnum.Standing;
        yield return new WaitForSeconds(shootCooldown - shootWalkCooldown);
        shootAllowed = true;
    }

    public void RunAway(Vector3 target)
    {
        Vector3 distance = transform.position - target;
        agent.SetDestination(transform.position + distance.normalized * lenghtOfRun);
        if (runAwayCooldown > 0)
        {
            StartCoroutine(RunAwayCooldown());
        }
    }

    public IEnumerator RunAwayCooldown()
    {
        runAwayAllowed = false;
        yield return new WaitForSeconds(runAwayCooldown);
        runAwayAllowed = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
