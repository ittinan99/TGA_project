using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToTarget : ActionNode
{
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 3.0f;

    private EnemyController enemyController;
    protected override void OnStart() 
    {
        enemyController = context.transform.GetComponent<EnemyController>();

        if (blackboard.Target == null) { return; }

        context.agent.stoppingDistance = enemyController.EnemyInfo.AttackDistance;
        context.agent.destination = blackboard.Target.transform.position;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;

        if (blackboard.IsAggro)
        {
            context.agent.speed = enemyController.EnemyInfo.RunSpeed;
        }
        else
        {
            context.agent.speed = enemyController.EnemyInfo.WalkSpeed;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (blackboard.IsDead)
        {
            return State.Success;
        }

        if (blackboard.IsFreeze)
        {
            return State.Running;
        }

        if (blackboard.Target == null)
        {
            return State.Success;
        }
        else
        {
            context.agent.destination = blackboard.Target.transform.position;
            enemyController.distance = Vector3.Distance(context.transform.position, blackboard.Target.transform.position);
        }

        if (Vector3.Distance(context.transform.position, blackboard.Target.transform.position) < enemyController.EnemyInfo.AggroRange)
        {
            blackboard.IsAggro = true;
            context.agent.speed = enemyController.EnemyInfo.RunSpeed;
        }

        if (blackboard.IsAggro)
        {
            context.agent.speed = enemyController.EnemyInfo.RunSpeed;
            enemyController.SetRunAnimation(true);
        }
        else
        {
            context.agent.speed = enemyController.EnemyInfo.WalkSpeed;
            enemyController.SetWalkAnimation(true);
        }

        if (Vector3.Distance(context.transform.position, blackboard.Target.transform.position) <= enemyController.EnemyInfo.AttackDistance + 0.5f)
        {
            return State.Success;
        }

        if (context.agent.pathPending)
        {
            //TODO : move animation
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            //TODO : move animation
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            //TODO : move animation
            return State.Failure;
        }

        return State.Running;
    }
}
