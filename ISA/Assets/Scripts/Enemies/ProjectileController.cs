using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject shooter;

    [Header("Stats")]
    public Vector3 direction;
    public float speed;
    public int damage;
    public float lifespan;

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        lifespan -= Time.deltaTime;
        if(lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus ps = other.GetComponent<PlayerStatus>();
        if (ps != null)
        {
            HitPlayer(ps);
        }
        else if(other.gameObject == shooter)
        {
            return;
        }
        else
        {
            HitTerrain();
        }
    }

    private void HitPlayer(PlayerStatus playerStatus) 
    {
        print("hit player");
        playerStatus.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void HitTerrain()
    {
        print("hit terrain");
        Destroy(gameObject);
    }
}
