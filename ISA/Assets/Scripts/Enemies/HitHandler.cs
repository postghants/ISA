using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    private EnemyController enemyController;
    private ShootyEnemyController shootyEnemyController;
    public enum EnemyType { Enemy, ShootyEnemy }
    public EnemyType enemyType;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        shootyEnemyController = GetComponent<ShootyEnemyController>();
        if(enemyController != null)
        {
            enemyType = EnemyType.Enemy;
        }
        else if(shootyEnemyController != null)
        {
            enemyType = EnemyType.ShootyEnemy;
        }
    }

    public void TakeDamage(int damage)
    {
        switch (enemyType)
        {
            case EnemyType.Enemy:
                enemyController.TakeDamage(damage);
                break;
            case EnemyType.ShootyEnemy:
                shootyEnemyController.TakeDamage(damage);
                break;
            default:
                break;
        }
    }
}
