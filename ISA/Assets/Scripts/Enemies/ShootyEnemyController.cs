using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class ShootyEnemyController : EnemyController
{
    public enum ShootyStateEnum { Standing, Shooting, Running, Knockback }
    public ShootyStateEnum shootyState;

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


    public override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerStatus>().transform;

        shootyState = ShootyStateEnum.Standing;
        health = maxHealth;
    }

    public override void FixedUpdate()
    {
        if (!spawned) return;

        switch (shootyState)
        {
            case ShootyStateEnum.Standing:
                StandingBehaviour(); break;
            case ShootyStateEnum.Running:
                break;
            case ShootyStateEnum.Shooting:
                break;
                case ShootyStateEnum.Knockback: 
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
    public override void KnockbackBehaviour()
    {
        if (CheckGrounded() && stunnedTimer <= 0)
        {
            shootyState = ShootyStateEnum.Standing;
            agent.enabled = true;
            rb.isKinematic = true;
        }
        else
        {
            stunnedTimer -= Time.deltaTime;
            Mathf.Clamp(stunnedTimer, 0, Mathf.Infinity);
        }
    }

    public IEnumerator Shoot()
    {
        shootyState = ShootyStateEnum.Shooting;
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
        shootyState = ShootyStateEnum.Standing;
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

    public override void TakeKnockback(Vector3 force, float time)
    {
        if (!spawned) return;

        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(force);
        stunnedTimer = time;
        shootyState = ShootyStateEnum.Knockback;
    }
}
