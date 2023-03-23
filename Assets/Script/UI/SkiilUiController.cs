using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkiilUiController : MonoBehaviour
{

    SkillController skillController;

    #region Public
    [SerializeField] SkillInfoConfig firstSkillConfig;
    [SerializeField] SkillInfoConfig secondSkillConfig;
    [SerializeField] SkillInfoConfig thirdSkillConfig;

    [Space(5)]
    [SerializeField] GameObject charddot;

    [SerializeField] Image[] skilImage;
    [SerializeField] GameObject[] chardcontainer;


    #endregion
    private void Awake()
    {
        skillController = GetComponentInParent<SkillController>();

    }

    private void Start()
    {
        OnSetImageSkil();
        CheckChardable();
    }


    void OnSetImageSkil()
    {
        skilImage[0].sprite = firstSkillConfig.SkillSprite;
        skilImage[1].sprite = secondSkillConfig.SkillSprite;
        skilImage[2].sprite = thirdSkillConfig.SkillSprite;
    }

    void CheckChardable()
    {
        if (firstSkillConfig.chardable.ToString() == "Ja")
        {
            chardcontainer[0].SetActive(true);
            for(int i = 0; i < firstSkillConfig.chardCount; i++)
            {
                Instantiate(charddot, chardcontainer[0].transform);
            }
            var ui = chardcontainer[0].GetComponent<RectTransform>();
            Rect rect = ui.rect;
            rect.width = ((10 * firstSkillConfig.chardCount) + 5);
            ui.sizeDelta = new Vector2(((10 * firstSkillConfig.chardCount) + 5), rect.height);

        }
        else { chardcontainer[1].SetActive(false); }

        if (secondSkillConfig.chardable.ToString() == "Ja")
        {
            chardcontainer[1].SetActive(true);
            for (int i = 0; i < secondSkillConfig.chardCount; i++)
            {
                Instantiate(charddot, chardcontainer[1].transform);
            }
            var ui = chardcontainer[1].GetComponent<RectTransform>();
            Rect rect = ui.rect;
            rect.width = ((10 * secondSkillConfig.chardCount) + 5);
            ui.sizeDelta = new Vector2(((10 * secondSkillConfig.chardCount) + 5), rect.height);
        }
        else { chardcontainer[1].SetActive(false); }

        if (thirdSkillConfig.chardable.ToString() == "Ja")
        {
            chardcontainer[2].SetActive(true);
            for (int i = 0; i < thirdSkillConfig.chardCount; i++)
            {
                Instantiate(charddot, chardcontainer[2].transform);
            }
            var ui = chardcontainer[2].GetComponent<RectTransform>();
            Rect rect = ui.rect;
            rect.width = ((10 * thirdSkillConfig.chardCount) + 5);
            ui.sizeDelta = new Vector2(((10 * thirdSkillConfig.chardCount) + 5), rect.height);

        }
        else { chardcontainer[2].SetActive(false); }
    }


    private void Update()
    {
        
    }


}
