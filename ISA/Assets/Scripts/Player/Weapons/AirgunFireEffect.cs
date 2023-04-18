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


    [Header("Attack Data")]
    public int damage;
    public float enemyKbForce;
    public float enemyKbTime;
    public float playerKbForce;
    public float fireCooldown;
    private float fireTimer = 0;

    public void Awake()
    {
        fpsCamera = FindObjectOfType<Camera>().transform;
        playerRb = fpsCamera.GetComponentInParent<Rigidbody>();
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (fireTimer == 0)
        {
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
