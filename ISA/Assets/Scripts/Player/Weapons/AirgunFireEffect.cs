using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AirgunFireEffect : MonoBehaviour, HandheldObject
{
    [Header("Hitbox Capsule")]
    public float hitStartOffset = 1;
    public float hitLength = 6;
    public float hitRadius = 1;
    public LayerMask hitMask;
    public LayerMask blockMask;
    [HideInInspector] public Transform fpsCamera;
    [HideInInspector] public Rigidbody playerRb;
    private playerLook playerLook;


    [Header("Attack Data")]
    public int damage;
    public float enemyKbForce;
    public float enemyKbTime;
    public float playerKbForce;
    public float fireCooldown;
    private float fireTimer = 0;
    public float shakeDuration;
    public float shakeIntensity;

    public void Awake()
    {
        fpsCamera = Camera.main.transform;
        playerRb = fpsCamera.GetComponentInParent<Rigidbody>();
        playerLook = fpsCamera.GetComponentInParent<playerLook>();
    }

    public void FixedUpdate()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer < 0)
            {
                fireTimer = 0;
            }
        }
    }

    public void Shoot()
    {
        playerLook.ShakeCamera(shakeDuration, shakeIntensity);
        Vector3 hitStartVector = fpsCamera.TransformDirection(Vector3.forward) * hitStartOffset;
        Vector3 hitLengthVector = fpsCamera.TransformDirection(Vector3.forward) * (hitStartOffset + hitLength);
        Collider[] colliders = Physics.OverlapCapsule(hitStartVector, hitLengthVector, hitRadius, hitMask);
        foreach (Collider collider in colliders)
        {
            if (!Physics.Raycast(fpsCamera.transform.position, collider.transform.position, Vector3.Distance(fpsCamera.transform.position, collider.transform.position), blockMask))
            {
                HitHandler target = collider.GetComponent<HitHandler>();
                target.TakeDamage(damage);
                Vector3 force = (collider.transform.position - fpsCamera.transform.position).normalized * enemyKbForce;
                target.TakeKnockback(force, enemyKbTime);
            }
        }
        Vector3 playerForce = fpsCamera.TransformDirection(Vector3.back) * playerKbForce;
        playerRb.AddForce(playerForce);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (fireTimer == 0)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    public void OnAttachedCarrier(carrierSystem carrier)
    {
    }

    public void OnEquip()
    {
    }


    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
    }

    public void OnUnequip()
    {
    }
}
