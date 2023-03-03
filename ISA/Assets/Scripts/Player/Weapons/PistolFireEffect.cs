using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolFireEffect : MonoBehaviour, HandheldObject
{
    public LayerMask hitMask;
    public Transform fpsCamera;

    public int damage = 5;

    private RaycastHit lastHitInfo;

    public void Awake()
    {
        fpsCamera = FindObjectOfType<Camera>().transform;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(Physics.Raycast(fpsCamera.position, fpsCamera.TransformDirection(Vector3.forward), out lastHitInfo, 100, hitMask))
        {
            print("Hit");
            HitHandler target = lastHitInfo.collider.gameObject.GetComponent<HitHandler>();
            target.TakeDamage(damage);
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
