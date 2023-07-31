using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolFireEffect : MonoBehaviour, HandheldObject
{
    public LayerMask hitMask;
    [HideInInspector] public Transform fpsCamera;
    public GameObject bulletHole;

    public int damage;
    public float fireCooldown;
    public float maxRange = 100;
    private float fireTimer = 0;

    private RaycastHit lastHitInfo;

    public void Awake()
    {
        fpsCamera = Camera.main.transform;
    }

    public void FixedUpdate()
    {
        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            if(fireTimer < 0)
            {
                fireTimer = 0;
            }
        }
    }

    public void Shoot()
    {
        if (Physics.Raycast(fpsCamera.position, fpsCamera.TransformDirection(Vector3.forward), out lastHitInfo, maxRange, hitMask))
        {
            HitHandler target = lastHitInfo.collider.gameObject.GetComponent<HitHandler>();
            if (target != null)
            {
                target.TakeDamage(damage);
                print("Hit");
            }
            if (lastHitInfo.collider.gameObject.layer == 6)
            {
                if (bulletHole != null)
                {
                    GameObject bh = Instantiate(bulletHole, lastHitInfo.collider.transform);
                    bh.transform.position = lastHitInfo.point + lastHitInfo.normal * 0.001f;
                    bh.transform.rotation = Quaternion.FromToRotation(Vector3.up, lastHitInfo.normal);
                }
            }
        }
        fireTimer = fireCooldown;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (fireTimer == 0)
        {
            Shoot();
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
