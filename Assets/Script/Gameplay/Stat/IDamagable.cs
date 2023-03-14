using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable<T>
{
    float CurrentHealth { get; }
    void reduceHealth(T amount);
    IEnumerator RegenHealth();
}
public interface IStaminaUsable<T>
{

    float CurrentStamina { get; }
    void reduceStamina(T amount);
    IEnumerator RegenStamina();
    IEnumerator ReduceStaminaOverTime(T amount);
}
