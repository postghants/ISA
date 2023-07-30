using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyController> enemies = new List<EnemyController>();
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerStatus>() != null && !activated)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        foreach(EnemyController enemy in enemies)
        {
            enemy.spawned = true;
        }
        activated = true;
    }
}
