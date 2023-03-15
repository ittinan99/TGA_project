using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CurveBullet : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> allTarget;

    [SerializeField]
    private FollowProjectile followProjectile;

    private bool isHitTarget;

    [SerializeField]
    private GameObject currentTarget;
    private Coroutine curveBulletCoroutine;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void startCurveBullet(List<GameObject> allTarget)
    {
        this.allTarget = allTarget;
        curveBulletCoroutine = StartCoroutine(curveBullet());
    }

    IEnumerator curveBullet()
    {
        int i = 0;

        while (i < allTarget.Count)
        {
            currentTarget = allTarget[i];
            isHitTarget = false;

            followProjectile.OnTriggerEnterObjectCallback += OnHitCurrentTarget;
            followProjectile.SetTarget(currentTarget);

            yield return new WaitUntil(() => isHitTarget);
            i++;
        }

        curveBulletCoroutine = null;
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void OnHitCurrentTarget(GameObject gameObject)
    {
        if (gameObject.GetComponent<EnemyController>() == null) { return; }
        if (gameObject.GetComponent<EnemyController>().HeadPos != currentTarget) { return; }

        Debug.Log("hit current target");

        gameObject.GetComponent<AttackTarget>().receiveAttack(100);

        isHitTarget = true;
        followProjectile.OnTriggerEnterObjectCallback -= OnHitCurrentTarget;
    }
}
