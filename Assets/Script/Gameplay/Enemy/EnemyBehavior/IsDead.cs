using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsDead : ActionNode
{
    private EnemyController enemyController;
    protected override void OnStart() {
        enemyController = context.gameObject.GetComponent<EnemyController>();

        if (blackboard.isInitDelegate) { return; }

        enemyController.EnemyStat.OnEnemyDieCallback += OnEnemyDie;
        enemyController.OnEnemyFreezeCallback += OnEnemyFreeze;
        enemyController.UpdateEnemySpeedCallback += UpdateEnemySpeed;

        blackboard.isInitDelegate = true;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.IsFreeze)
        {
            return State.Running;
        }

        if (blackboard.IsDead)
        {
            enemyController.SetDieAnimation(true);

            return State.Failure;
        }
        else
        {
            return State.Success;
        }
    }

    private void OnEnemyDie()
    {
        blackboard.IsDead = true;
        context.agent.enabled = false;
    }

    private void OnEnemyFreeze(bool status)
    {
        blackboard.IsFreeze = status;

        if (!context.agent.enabled) { return; }

        context.agent.isStopped = status;
    }

    private void UpdateEnemySpeed(float speed)
    {
        context.agent.speed = speed;
    }
}
