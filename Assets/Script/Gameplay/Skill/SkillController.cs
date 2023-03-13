using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGA.GameData;
using TGA.Gameplay;
using UnityEngine.UI;
using System.Linq;

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

    void Start()
    {
        
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

        var allTarget = GameObject.FindGameObjectsWithTag("Enemy");

        var inSightTarget = new List<GameObject>(allTarget).FindAll((x) => x.GetComponent<MeshRenderer>().isVisible);
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
    }

    public void UseThirdSkill()
    {
        Debug.Log("Use Third Skill !!");
        ThirdSkillCooldownCoroutine = StartCoroutine(ThirdSkillCooldown());

        //TEST Curve Bullet;

        var allTarget = GameObject.FindGameObjectsWithTag("Enemy");

        var inSightTarget = new List<GameObject>(allTarget).FindAll((x) => x.GetComponent<MeshRenderer>().isVisible);

        var enemyTarget = new List<EnemyController>();

        var headTarget = new List<GameObject>();

        foreach(var target in inSightTarget)
        {
            enemyTarget.Add(target.GetComponent<EnemyController>());
        }

        enemyTarget = enemyTarget.OrderBy(x => x.distance).ToList();

        foreach (var target in enemyTarget)
        {
            headTarget.Add(target.HeadPos);
        }

        CurveBullet a = Instantiate(CurveBullet, transform.position, Quaternion.identity).GetComponent<CurveBullet>();
        a.startCurveBullet(headTarget);
    }
}
