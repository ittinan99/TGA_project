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

    [Header("collider")]
    [SerializeField]
    private List<EnemyBoxCollider> hurtBoxColliderList;

    [SerializeField]
    private List<Collider> handColliderList;

    [SerializeField]
    private List<Collider> footColliderList;
    [Space(10)]

    [Header("Debug Value")]
    public GameObject target;
    public float distance;

    public bool IsAttacking;

    public delegate void OnEnemyFreeze(bool status);
    public OnEnemyFreeze OnEnemyFreezeCallback;

    public delegate void UpdateEnemySpeed(float speed);
    public UpdateEnemySpeed UpdateEnemySpeedCallback;

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
        SetAnimationSpeed(1f);

        if (hurtBoxColliderList.Count <= 0) { return; }

        foreach (EnemyBoxCollider enemyBoxCollider in hurtBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.AddListener(OnHurtBoxTriggerEnter);
        }

        setupGuid();
    }

    void setupGuid()
    {
        var testGuid = Guid.NewGuid();

        //Guid = testGuid;

        if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }

        pv.RPC("RPC_SetupGuid", RpcTarget.All, testGuid.ToString());
    }

    [PunRPC]
    void RPC_SetupGuid(string guidString)
    {
        Guid = new Guid(guidString);
        Debug.Log($"{gameObject.name} : {Guid}");
    }

    void Update()
    {
        if (target == null) { return; }

        distance = Vector3.Distance(transform.position, target.transform.position);
    }

    private void OnHurtBoxTriggerEnter(Collider col, GameObject gameObject)
    {
        if (!col.CompareTag("Player")) { return; }

        if (!IsAttacking) { return; }

        Debug.Log($"{col.gameObject.name} โดนตี เพราะ โดน {gameObject}");

        col.gameObject.GetComponent<IDamageable>()?.TakeDamage(10);
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

    public void SetAnimationSpeed(float speed)
    {
        animController.SetFloat("multiplier",speed);
    }

    public bool IsCurrentAnimationStateIsName(string name)
    {
        return animController.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    #endregion

    #region AnimationEvent
    public void EnableHandCollider()
    {
        foreach(var handCollider in handColliderList)
        {
            handCollider.enabled = true;
        }
    }

    public void DisableHandCollider()
    {
        foreach (var handCollider in handColliderList)
        {
            handCollider.enabled = false;
        }
    }

    public void EnableFootCollider()
    {
        foreach (var footCollider in footColliderList)
        {
            footCollider.enabled = false;
        }
    }

    public void DisableFootCollider()
    {
        foreach (var footCollider in footColliderList)
        {
            footCollider.enabled = true;
        }
    } 
    #endregion

    public void FreezeEnemy(float duration)
    {
        StartCoroutine(FreezeEnemyFor(duration));
    }

    IEnumerator FreezeEnemyFor(float duration)
    {
        var timer = 2f;
        var multiplier = timer;

        while(timer > 0f)
        {
            timer -= Time.deltaTime;
            SetAnimationSpeed(timer / multiplier);
            UpdateEnemySpeedCallback?.Invoke((timer / multiplier) * enemyInfo.RunSpeed);
            yield return null;
        }

        SetAnimatorStatus(false);
      
        OnEnemyFreezeCallback?.Invoke(true);

        yield return new WaitForSeconds(duration);

        UpdateEnemySpeedCallback?.Invoke(enemyInfo.RunSpeed);
        SetAnimationSpeed(1f);
        SetAnimatorStatus(true);
        OnEnemyFreezeCallback?.Invoke(false);
    }
}
