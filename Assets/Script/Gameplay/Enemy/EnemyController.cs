using System.Collections;
using System.Collections.Generic;
using TGA.Gameplay;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyInfo enemyInfo;
    public EnemyInfo EnemyInfo => enemyInfo;

    [SerializeField]
    private float currentHealth;
    public float CurrentHealth => currentHealth;

    [SerializeField]
    private GameObject headPos;
    public GameObject HeadPos => headPos;

    public float distance;
    void Start()
    {
        setupVariable();
    }

    void setupVariable()
    {
        currentHealth = enemyInfo.MaxHealth;
    }

    void Update()
    {
        
    }
}
