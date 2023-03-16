using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBoxCollider : MonoBehaviour
{
    public UnityEvent<Collider,GameObject> onTriggerEnter;
    public UnityEvent<Collision,GameObject> onCollisionEnter;

    void OnTriggerEnter(Collider col)
    {
        if (onTriggerEnter != null) onTriggerEnter.Invoke(col,this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (onCollisionEnter != null) onCollisionEnter.Invoke(collision, this.gameObject);
    }
}
