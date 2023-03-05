using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGround : MonoBehaviour
{
    public bool onGround;
    public float groundRadius;
    public LayerMask groundLayerMask;

    public void Update()
    {
        onGround = Physics.CheckSphere(transform.position, groundRadius, groundLayerMask);
    }



    public bool GetOnGround()
    {
        return onGround;
    }
}
