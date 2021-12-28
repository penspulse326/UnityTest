using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("最大血量")]
    [SerializeField] private float maxHealth = 10f;
    [Header("當前血量")] 
    [SerializeField] private float currentHealth;

    //當受到攻擊時觸發的委派事件
    public event Action onDamage;
    //當受到治療效果時觸發的委派事件 並回傳float變數
    public event Action<float> onHealth;
    //當人物死亡時觸發的委派事件
    public event Action onDie;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;   
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        if(isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if(currentHealth > 0)
        {
            onDamage?.Invoke();
        }

        if(currentHealth <= 0)
        {
            HealthDead();
        }
    }

    private void HealthDead()
    {
        if(isDead) return;

        if(currentHealth <= 0)
        {
            isDead = true;

            onDie?.Invoke();
        }
    }
}
