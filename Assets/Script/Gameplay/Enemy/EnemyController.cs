using System.Collections;
using System.Collections.Generic;
using TGA.Gameplay;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TheKiwiCoder;
using System;

public class EnemyController : MonoBehaviour
{
    [Header("EnemyInfo")]
    [SerializeField]
    private EnemyInfo enemyInfo;
    public EnemyInfo EnemyInfo => enemyInfo;

    public Guid Guid; //TODO : add to custom property
    [Space(10)]

    [Header("EnemyStat")]
    [SerializeField]
    private EnemyStat enemyStat;
    public EnemyStat EnemyStat => enemyStat;
    [Space(10)]

    [Header("Component")]
    [SerializeField]
    private GameObject headPos;
    public GameObject HeadPos => headPos;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    public SkinnedMeshRenderer Renderer => meshRenderer;
    [Space(10)]

    [Header("Animation")]
    [SerializeField]
    private Animator animController;
    [Space(10)]

    [Header("BehaviorTree")]
    [SerializeField]
    private BehaviourTreeRunner btr;
    [Space(10)]

    [Header("Debug Value")]
    public float distance;

    public delegate void OnEnemyFreeze(bool status);
    public OnEnemyFreeze OnEnemyFreezeCallback;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        setupVariable();
    }

    void Start()
    {

    }

    void setupVariable()
    {
        enemyStat.SetupVariable(enemyInfo.MaxHealth);

        setupGuid();
    }

    void setupGuid()
    {
        var testGuid = Guid.NewGuid();

        //Guid = testGuid;

        pv.RPC("RPC_SetupGuid", RpcTarget.All, testGuid.ToString());
    }

    [PunRPC]
    void RPC_SetupGuid(string guidString)
    {
        if (!pv.IsMine) { return; }

        Guid = new Guid(guidString);
        Debug.Log($"{gameObject.name} : {Guid}");
    }

    void Update()
    {
        
    }

    #region Animation

    public void SetIdleAnimation(bool status)
    {
        animController.SetBool("idle", status);
    }

    public void SetWalkAnimation(bool status)
    {
        animController.SetBool("walk", status);
        animController.SetBool("run", !status);
    }

    public void SetRunAnimation(bool status)
    {
        animController.SetBool("run", status);
        animController.SetBool("walk", !status);
    }

    public void StopMoving()
    {
        animController.SetBool("run", false);
        animController.SetBool("walk", false);
        animController.SetBool("idle", true);
    }

    public void StartMoving()
    {
        animController.SetBool("idle", false);
    }

    public void SetDieAnimation(bool status)
    {
        animController.SetBool("die", status);
    }

    public void SetScreamTrigger()
    {
        animController.SetTrigger("scream");
    }

    public void PlayAnimation(AnimationClip animationClip)
    {
        animController.Play(animationClip.name);
    }

    public void SetRootNodeStatus(bool status)
    {
        animController.applyRootMotion = status;
    }

    public void SetAnimatorStatus(bool status)
    {
        animController.enabled = status;
    }

    public bool IsCurrentAnimationStateIsName(string name)
    {
        return animController.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    #endregion

    public void FreezeEnemy(float duration)
    {
        StartCoroutine(FreezeEnemyFor(duration));
    }

    IEnumerator FreezeEnemyFor(float duration)
    {
        SetAnimatorStatus(false);
        OnEnemyFreezeCallback?.Invoke(true);

        yield return new WaitForSeconds(duration);

        SetAnimatorStatus(true);
        OnEnemyFreezeCallback?.Invoke(false);
    }
}
