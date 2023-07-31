using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerController : MonoBehaviour
{
    [HideInInspector] public float timer;

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            Destroy(gameObject);
    }
}
