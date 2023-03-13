using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsTargetNearby : ActionNode
{
    public float DetectedRanged;
    private EnemyController enemyController;

    protected override void OnStart() 
    {
        enemyController = context.transform.GetComponent<EnemyController>();
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() {
        if (CheckIsPlayerNearBy())
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
    public bool CheckIsPlayerNearBy()
    {
        List<GameObject> PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject nearestTarget = GetClosestEnemy(PlayerList, context.transform);
        if (Vector3.Distance(context.gameObject.transform.position, nearestTarget.transform.position) < enemyController.EnemyInfo.DetectRange)
        {
            if(blackboard.Target != nearestTarget)
            {
                blackboard.Target = nearestTarget;
                Debug.Log("Enemy Spotted");
            }
            return true;
        }
        else
        {
            blackboard.Target = null;
            return false;
        }
    }
    private GameObject GetClosestEnemy(List<GameObject> enemies, Transform fromThis)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

}
