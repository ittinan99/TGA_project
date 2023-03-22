using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TGA.Utilities;
using TGA.Gameplay;
using Photon.Pun;

public class EnemyStat : AttackTarget, IDamagable<float>
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField]
    private float currentHealth;

    public delegate void OnEnemyDie();
    public OnEnemyDie OnEnemyDieCallback;

    PhotonView pv;

    private bool isDead;

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
        pv = GetComponent<PhotonView>();
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
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;

            OnEnemyDieCallback?.Invoke();
        }
    }
}
