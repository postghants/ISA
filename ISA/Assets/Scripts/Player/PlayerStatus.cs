using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public Image healthBar;

    private void Awake()
    {
        health = maxHealth;
        SetHealthBar((float)health / maxHealth);
    }

    private void FixedUpdate()
    {
        if(health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Mathf.Clamp(health, 0, maxHealth);
        SetHealthBar((float)health / maxHealth);
    }

    public void HealDamage(int heal)
    {
        health += heal;
        Mathf.Clamp(health, 0, maxHealth);
        SetHealthBar((float)health / maxHealth);
    }

    public void SetHealthBar(float value)
    {
        healthBar.fillAmount = value;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
