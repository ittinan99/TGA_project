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
    private SkillInfo firstSkill;
    [SerializeField] private SkillInfoConfig firstSkillConfig;

    private SkillInfo secondSkill;
    [SerializeField] private SkillInfoConfig secondSkillConfig;

    private SkillInfo thirdSkill;
    [SerializeField] private SkillInfoConfig thirdSkillConfig;

    [Header("Input")]
    [Space(5)]

    [SerializeField] private KeyCode firstSkillButton;
    [SerializeField] private KeyCode secondSkillButton;
    [SerializeField] private KeyCode thirdSkillButton;

    [Header("UI")]
    [Space(5)]
    [SerializeField] private GameObject chardDot;
    [SerializeField] private Image FirstSkillImage;
    [SerializeField] private Image FirstSkillImageCoolDown;
    [SerializeField] private GameObject FirstSkillChardContainer;
    [SerializeField] private Image SecondSkillImage;
    [SerializeField] private Image SecondSkillImageCoolDown;
    [SerializeField] private GameObject SecondSkillChardContainer;
    [SerializeField] private Image ThirdSkillImage;
    [SerializeField] private Image ThirdSkillImageCoolDown;
    [SerializeField] private GameObject ThirdSkillChardContainer;

    private bool isFirstSkillOnCooldown;
    private bool isSecondSkillOnCooldown;
    private bool isThirdSkillOnCooldown;

    private Coroutine FirstSkillCooldownCoroutine;
    private Coroutine SecondSkillCooldownCoroutine;
    private Coroutine ThirdSkillCooldownCoroutine;

    [SerializeField] private GameObject CurveBullet;

    [SerializeField] private GameObject FreezeRoarVFX;

    [SerializeField] private int inSightEnemyCount;
    private List<GameObject> EnemyList;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        OnSetImageSkil();
        CheckChardable();
    }

    void Update()
    {
        UpdateInSightEnemy();

        if (!pv.IsMine) { return; }

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

        FirstSkillImageCoolDown.fillAmount = 0;
        var timer = firstSkillConfig.Cooldown;

        while(timer > 0)
        {
            FirstSkillImageCoolDown.fillAmount = timer / firstSkillConfig.Cooldown;
            timer -= Time.deltaTime;
            yield return null;
        }

        isFirstSkillOnCooldown = false;
        FirstSkillCooldownCoroutine = null;
    }

    IEnumerator SecondSkillCooldown()
    {
        //yield return new WaitUntil(() => secondSkillUsed)      If we have placing object skill need to check is object has deployed before cooldown

        isSecondSkillOnCooldown = true;
        SecondSkillImageCoolDown.fillAmount = 0;
        var timer = secondSkillConfig.Cooldown;

        while (timer > 0)
        {
            SecondSkillImageCoolDown.fillAmount = timer / secondSkillConfig.Cooldown;
            timer -= Time.deltaTime;
            yield return null;
        }

        isSecondSkillOnCooldown = false;
        SecondSkillCooldownCoroutine = null;
    }

    IEnumerator ThirdSkillCooldown()
    {
        //yield return new WaitUntil(() => secondSkillUsed)      If we have placing object skill need to check is object has deployed before cooldown

        isThirdSkillOnCooldown = true;
        ThirdSkillImageCoolDown.gameObject.SetActive(true);
        ThirdSkillImageCoolDown.fillAmount = 0;
        var timer = thirdSkillConfig.Cooldown;

        while (timer > 0)
        {
            ThirdSkillImageCoolDown.fillAmount = timer / thirdSkillConfig.Cooldown;
            timer -= Time.deltaTime;
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

        //CurveBullet a = PhotonNetwork.Instantiate(Path.Combine("Photonprefabs", "CurveBullet"), transform.position, Quaternion.identity).GetComponent<CurveBullet>();
        CurveBullet a = Instantiate(CurveBullet, transform.position, Quaternion.identity).GetComponent<CurveBullet>();
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

        var enemyGuidList = new List<Guid>();

        foreach (var enemy in enemyTarget)
        {
            enemyGuidList.Add(enemy.Guid);
        }

        var data = new TargetEnemyData(enemyGuidList).Serialize();

        pv.RPC("RPC_useFreezeRoar", RpcTarget.All, data);
    }

    [PunRPC]
    void RPC_useFreezeRoar(string data)
    {
        var verifyData = BaseNetworkData.Deserialize<TargetEnemyData>(data);
        var targetEnemyGuidList = verifyData.TargetEnemyList;

        var enemyTarget = new List<EnemyController>();

        foreach (var enemyGuid in targetEnemyGuidList)
        {
            var enemy = EnemyList.Find(x => x.GetComponent<EnemyController>().Guid == enemyGuid);
            enemyTarget.Add(enemy.GetComponent<EnemyController>());
        }

        foreach (var target in enemyTarget)
        {
            target.FreezeEnemy(4f);
        }

        StartCoroutine(ShowFreezeRoarVFX());
    }

    IEnumerator ShowFreezeRoarVFX()
    {
        GameObject b = Instantiate(FreezeRoarVFX, transform.position, transform.rotation);

        yield return new WaitForSeconds(4f);

        Destroy(b.gameObject);
    }

    #region UI
    void OnSetImageSkil()
    {
        FirstSkillImage.sprite = firstSkillConfig.SkillSprite;
        SecondSkillImage.sprite = secondSkillConfig.SkillSprite;
        ThirdSkillImage.sprite = thirdSkillConfig.SkillSprite;

        FirstSkillImageCoolDown.sprite = firstSkillConfig.SkillSprite;
        SecondSkillImageCoolDown.sprite = secondSkillConfig.SkillSprite;
        ThirdSkillImageCoolDown.sprite = thirdSkillConfig.SkillSprite;

        FirstSkillImageCoolDown.fillAmount = 0;
        SecondSkillImageCoolDown.fillAmount = 0;
        ThirdSkillImageCoolDown.fillAmount = 0;
    }

    void CheckChardable()
    {
        if (firstSkillConfig.chardable.ToString() == "Ja")
        {
            FirstSkillChardContainer.SetActive(true);
            for (int i = 0; i < firstSkillConfig.chardCount; i++)
            {
                Instantiate(chardDot, FirstSkillChardContainer.transform);
            }
            var ui = FirstSkillChardContainer.GetComponent<RectTransform>();
            Rect rect = ui.rect;
            rect.width = ((10 * firstSkillConfig.chardCount) + 5);
            ui.sizeDelta = new Vector2(((10 * firstSkillConfig.chardCount) + 5), rect.height);

        }
        else { SecondSkillChardContainer.SetActive(false); }

        if (secondSkillConfig.chardable.ToString() == "Ja")
        {
            SecondSkillChardContainer.SetActive(true);
            for (int i = 0; i < secondSkillConfig.chardCount; i++)
            {
                Instantiate(chardDot, SecondSkillChardContainer.transform);
            }
            var ui = SecondSkillChardContainer.GetComponent<RectTransform>();
            Rect rect = ui.rect;
            rect.width = ((10 * secondSkillConfig.chardCount) + 5);
            ui.sizeDelta = new Vector2(((10 * secondSkillConfig.chardCount) + 5), rect.height);
        }
        else { SecondSkillChardContainer.SetActive(false); }

        if (thirdSkillConfig.chardable.ToString() == "Ja")
        {
            ThirdSkillChardContainer.SetActive(true);
            for (int i = 0; i < thirdSkillConfig.chardCount; i++)
            {
                Instantiate(chardDot, ThirdSkillChardContainer.transform);
            }
            var ui = ThirdSkillChardContainer.GetComponent<RectTransform>();
            Rect rect = ui.rect;
            rect.width = ((10 * thirdSkillConfig.chardCount) + 5);
            ui.sizeDelta = new Vector2(((10 * thirdSkillConfig.chardCount) + 5), rect.height);

        }
        else { ThirdSkillChardContainer.SetActive(false); }
    }
    #endregion
}
