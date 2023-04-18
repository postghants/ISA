using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HitHandler : MonoBehaviour
{
    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void TakeDamage(int damage)
    {
        enemyController.TakeDamage(damage);
    }

    public void TakeKnockback(Vector3 force, float time)
    {
        enemyController.TakeKnockback(force, time);
    }
}
