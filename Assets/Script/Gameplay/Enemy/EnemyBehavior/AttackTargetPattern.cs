using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using TGA.Utilities;

public class AttackTargetPattern : ActionNode
{
    private Coroutine attackCoroutine;
    private CoroutineHelper coroutineHelper;
    private EnemyController enemyController;

    private AnimationClip currentAnimationClip;

    protected override void OnStart()
    {

        enemyController = context.gameObject.GetComponent<EnemyController>();
        coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();

        if (enemyController.EnemyInfo.AttackAnimation.Count > 0)
        {
            var allAttackAnimation = enemyController.EnemyInfo.AttackAnimation;
            float randNum = Random.Range(0, (allAttackAnimation.Count - 1) * 10);
            currentAnimationClip = allAttackAnimation[Mathf.RoundToInt(randNum / 10)];
            attackCoroutine = coroutineHelper.Play(attackingCoroutine(currentAnimationClip.length));
            blackboard.IsAttacking = true;

            Debug.Log($"ATTACK TARGET WITH {currentAnimationClip.name}");
        }  
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (blackboard.IsDead)
        {
            attackCoroutine = null;
            blackboard.IsAttacking = false;
            enemyController.StopMoving();

            return State.Success;
        }

        if (blackboard.IsFreeze)
        {
            return State.Running;
        }

        if (!blackboard.IsAttacking)
        {
            return State.Success;
        }
        else
        {
            var lookPos = blackboard.Target.transform.position - context.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, Time.deltaTime * 20);

            return State.Running;
        }
    }

    private IEnumerator attackingCoroutine(float length)
    {
        enemyController.StopMoving();
        enemyController.IsAttacking = true;
        
        if (context.agent.enabled)
        {
            context.agent.isStopped = true;
        }

        yield return new WaitForSeconds(0.25f);

        enemyController.SetRootNodeStatus(true);
        enemyController.PlayAnimation(currentAnimationClip);

        yield return new WaitForSeconds(length);

        enemyController.SetRootNodeStatus(false);
        enemyController.StartMoving();
        enemyController.IsAttacking = false;

        if (context.agent.enabled)
        {
            context.agent.isStopped = false;
        }

        attackCoroutine = null;
        blackboard.IsAttacking = false;
    }
}
