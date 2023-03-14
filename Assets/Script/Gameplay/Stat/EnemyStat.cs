using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TGA.Utilities;
using TGA.Gameplay;

public class EnemyStat : AttackTarget, IDamagable<float>
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    private float currentHealth;

    public delegate void OnEnemyDie();
    public OnEnemyDie OnEnemyDieCallback;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }

    private void Awake()
    {
        
    }

    public void SetupVariable(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void reduceHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnEnemyDieCallback?.Invoke();
        }
    }

    public IEnumerator RegenHealth()
    {
        throw new System.NotImplementedException();
    }

    public override void receiveAttack(float damage)
    {
        reduceHealth(damage);
    }
}
