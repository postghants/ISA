using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolFireEffect : MonoBehaviour, HandheldObject
{
    public LayerMask hitMask;
    [HideInInspector] public Transform fpsCamera;

    public int damage;
    public float fireCooldown;
    private float fireTimer = 0;

    private RaycastHit lastHitInfo;

    public void Awake()
    {
        fpsCamera = FindObjectOfType<Camera>().transform;
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (fireTimer == 0)
        {
            if (Physics.Raycast(fpsCamera.position, fpsCamera.TransformDirection(Vector3.forward), out lastHitInfo, 100, hitMask))
            {
                HitHandler target = lastHitInfo.collider.gameObject.GetComponent<HitHandler>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    print("Hit");
                }
            }
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
