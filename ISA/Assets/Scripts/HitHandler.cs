using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    public EnemyController enemyController;
    public void TakeDamage(int damage)
    {
        enemyController.TakeDamage(damage);
    }
}
