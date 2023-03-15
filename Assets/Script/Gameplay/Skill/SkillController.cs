using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGA.GameData;
using TGA.Gameplay;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using TGA.Network;
using System;
using System.IO;

public class SkillController : MonoBehaviour
{
    [SerializeField]
    private SkillInfo firstSkill;

    [SerializeField]
    private SkillInfo secondSkill;

    [SerializeField]
    private SkillInfo thirdSkill;

    [Header("Input")]
    [Space(5)]

    [SerializeField]
    private KeyCode firstSkillButton;
    [SerializeField]
    private KeyCode secondSkillButton;
    [SerializeField]
    private KeyCode thirdSkillButton;

    [Header("UI")]
    [Space(5)]
    [SerializeField]
    private Image FirstSkillImage;
    [SerializeField]
    private Image SecondSkillImage;
    [SerializeField]
    private Image ThirdSkillImage;

    private bool isFirstSkillOnCooldown;
    private bool isSecondSkillOnCooldown;
    private bool isThirdSkillOnCooldown;

    private Coroutine FirstSkillCooldownCoroutine;
    private Coroutine SecondSkillCooldownCoroutine;
    private Coroutine ThirdSkillCooldownCoroutine;

    [SerializeField]
    private GameObject CurveBullet;

    [SerializeField]
    private int inSightEnemyCount;
    private List<GameObject> EnemyList;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
       if (!pv.IsMine)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(firstSkillButton) && !isFirstSkillOnCooldown)
        {
            UseFirstSkill();
        }

        if (Input.GetKeyDown(secondSkillButton) && !isSecondSkillOnCooldown)
        {
            UseSecondSkill();
        }

        if (Input.GetKeyDown(thirdSkillButton) && !isThirdSkillOnCooldown)
        {
            UseThirdSkill();
        }

        UpdateInSightEnemy();
    }

    private void UpdateInSightEnemy()
    {
        var allTarget = GameObject.FindGameObjectsWithTag("Enemy");

        EnemyList = new List<GameObject>(allTarget);

        if (allTarget.Length == 0) { return; }

        var inSightTarget = new List<GameObject>(allTarget).FindAll((x) => x.GetComponent<EnemyController>().Renderer.isVisible);
        inSightEnemyCount = inSightTarget.Count;
    }

    IEnumerator FirstSkillCooldown()
    {
        //yield return new WaitUntil(() => firstSkillUsed)      If we have placing object skill need to check is object has deployed before cooldown

        isFirstSkillOnCooldown = true;
        FirstSkillImage.fillAmount = 0;
        var timer = 0f;

        while(timer < firstSkill.Cooldown)
        {
            FirstSkillImage.fillAmount = timer / firstSkill.Cooldown;
            timer += Time.deltaTime;
            yield return null;
        }

        isFirstSkillOnCooldown = false;
        FirstSkillCooldownCoroutine = null;
    }

    IEnumerator SecondSkillCooldown()
    {
        //yield return new WaitUntil(() => secondSkillUsed)      If we have placing object skill need to check is object has deployed before cooldown

        isSecondSkillOnCooldown = true;
        SecondSkillImage.fillAmount = 0;
        var timer = 0f;

        while (timer < secondSkill.Cooldown)
        {
            SecondSkillImage.fillAmount = timer / secondSkill.Cooldown;
            timer += Time.deltaTime;
            yield return null;
        }

        isSecondSkillOnCooldown = false;
        SecondSkillCooldownCoroutine = null;
    }

    IEnumerator ThirdSkillCooldown()
    {
        //yield return new WaitUntil(() => secondSkillUsed)      If we have placing object skill need to check is object has deployed before cooldown

        isThirdSkillOnCooldown = true;
        ThirdSkillImage.fillAmount = 0;
        var timer = 0f;

        while (timer < thirdSkill.Cooldown)
        {
            ThirdSkillImage.fillAmount = timer / thirdSkill.Cooldown;
            timer += Time.deltaTime;
            yield return null;
        }

        isThirdSkillOnCooldown = false;
        ThirdSkillCooldownCoroutine = null;
    }

    //FOR PROTOTYPE ONLY

    public void UseFirstSkill()
    {
        Debug.Log("Use First Skill !!");
        FirstSkillCooldownCoroutine = StartCoroutine(FirstSkillCooldown());
    }

    public void UseSecondSkill()
    {
        Debug.Log("Use Second Skill !!");
        SecondSkillCooldownCoroutine = StartCoroutine(SecondSkillCooldown());

        useFreezeRoar();
    }

    public void UseThirdSkill()
    {
        Debug.Log("Use Third Skill !!");
        ThirdSkillCooldownCoroutine = StartCoroutine(ThirdSkillCooldown());

        //TEST Curve Bullet;

        useCurveBullet();
    }

    [PunRPC]
    void RPC_useCurveBullet(string data)
    {
        var verifyData = BaseNetworkData.Deserialize<TargetEnemyData>(data);
        var targetEnemyGuidList = verifyData.TargetEnemyList;

        var enemyTarget = new List<EnemyController>();

        var headTarget = new List<GameObject>();

        foreach(var enemyGuid in targetEnemyGuidList)
        {
            var enemy = EnemyList.Find(x => x.GetComponent<EnemyController>().Guid == enemyGuid);
            enemyTarget.Add(enemy.GetComponent<EnemyController>());
        }

        foreach (var target in enemyTarget)
        {
            headTarget.Add(target.HeadPos);
        }

        CurveBullet a = PhotonNetwork.Instantiate(Path.Combine("Photonprefabs", "CurveBullet"), transform.position, Quaternion.identity).GetComponent<CurveBullet>();
        a.startCurveBullet(headTarget);
    }

    private void useCurveBullet()
    {
        if (!pv.IsMine) { return; }

        var allTarget = GameObject.FindGameObjectsWithTag("Enemy");

        var inSightTarget = new List<GameObject>(allTarget).FindAll((x) => x.GetComponent<EnemyController>().Renderer.isVisible);

        var enemyTarget = new List<EnemyController>();

        var headTarget = new List<GameObject>();

        foreach (var target in inSightTarget)
        {
            enemyTarget.Add(target.GetComponent<EnemyController>());
        }

        enemyTarget = enemyTarget.OrderBy(x => x.distance).ToList();

        //foreach (var target in enemyTarget)
        //{
        //    headTarget.Add(target.HeadPos);
        //}

        //CurveBullet a = Instantiate(CurveBullet, transform.position, Quaternion.identity).GetComponent<CurveBullet>();
        //a.startCurveBullet(headTarget);

        var enemyGuidList = new List<Guid>();

        foreach(var enemy in enemyTarget)
        {
            enemyGuidList.Add(enemy.Guid);
        }

        var data = new TargetEnemyData(enemyGuidList).Serialize();

        pv.RPC("RPC_useCurveBullet", RpcTarget.All, data);
    }

    private void useFreezeRoar()
    {
        var allTarget = GameObject.FindGameObjectsWithTag("Enemy");

        var inSightTarget = new List<GameObject>(allTarget).FindAll((x) => x.GetComponent<EnemyController>().Renderer.isVisible);

        var enemyTarget = new List<EnemyController>();

        foreach (var target in inSightTarget)
        {
            enemyTarget.Add(target.GetComponent<EnemyController>());
        }

        enemyTarget = enemyTarget.FindAll(x => x.distance < 15);

        foreach (var target in enemyTarget)
        {
            target.FreezeEnemy(4f);
        }
    }
}
