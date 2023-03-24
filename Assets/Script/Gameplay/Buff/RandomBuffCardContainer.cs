using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using TGA.Utilities;

namespace TGA.Gameplay
{
    public class RandomBuffCardContainer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        [SerializeField] private Sprite copperSprite;
        [SerializeField] private Sprite silverSprite;
        [SerializeField] private Sprite goldSprite;

        [SerializeField] private Image bannerImage;

        [Header("Good Buff")]

        public RandomBuffCard GoodBuffCard;
        [SerializeField] private TextMeshProUGUI goodBuffName;
        [SerializeField] private TextMeshProUGUI goodBuffDescription;
        [Space(10)]

        [Header("Bad Buff")]

        public RandomBuffCard BadBuffCard;
        [SerializeField] private TextMeshProUGUI badBuffName;
        [SerializeField] private TextMeshProUGUI badBuffDescription;

        public int Selected;
        private bool isClicked;

        private PhotonView pv;
        private RectTransform rt;
        private RandomBuffController randomBuffController;

        [SerializeField] private GameObject oneSelected;
        [SerializeField] private GameObject twoSelected;
        [SerializeField] private GameObject threeSelected;
        [SerializeField] private GameObject fourSelected;

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            rt = GetComponent<RectTransform>();
            randomBuffController = SharedContext.Instance.Get<RandomBuffController>();
        }

        private void Update()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            rt.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            rt.transform.localScale = Vector3.one;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isClicked)
            {
                pv.RPC("RPC_UpdateDeselected", RpcTarget.All);
            }
            else
            {
                isClicked = true;

                pv.RPC("RPC_UpdateSelected", RpcTarget.All);

                if (randomBuffController.selectedContainer == null)
                {
                    randomBuffController.selectedContainer = this;
                }
                else if (randomBuffController.selectedContainer != this)
                {
                    randomBuffController.selectedContainer.Deselected();
                    randomBuffController.selectedContainer = this;
                }
            }
        }

        [PunRPC]
        private void RPC_UpdateSelected()
        {
            Selected++;
            updateSelectedSprite();
        }

        public void Deselected()
        {
            isClicked = false;
            pv.RPC("RPC_UpdateDeselected", RpcTarget.All);
        }

        [PunRPC]
        private void RPC_UpdateDeselected()
        {
            if (Selected > 0)
            {
                Selected--;
                updateSelectedSprite();
            }
        }

        void updateSelectedSprite()
        {
            fourSelected.SetActive(false);
            threeSelected.SetActive(false);
            twoSelected.SetActive(false);
            oneSelected.SetActive(false);

            if (Selected == 4)
            {
                fourSelected.SetActive(true);
            }
            else if (Selected == 3)
            {
                threeSelected.SetActive(true);
            }
            else if(Selected == 2)
            {
                twoSelected.SetActive(true);
            }
            else if(Selected == 1)
            {
                oneSelected.SetActive(true);
            }
        }

        public void Populate(RandomBuffCard goodBuffCard, RandomBuffCard badBuffCard)
        {
            GoodBuffCard = goodBuffCard;
            BadBuffCard = badBuffCard;

            goodBuffName.text = goodBuffCard.Name;
            goodBuffDescription.text = goodBuffCard.Description;

            badBuffName.text = badBuffCard.Name;
            badBuffDescription.text = badBuffCard.Description;

            if (goodBuffCard.Rarity == 3)
            {
                bannerImage.sprite = copperSprite;
            }
            else if (goodBuffCard.Rarity == 2)
            {
                bannerImage.sprite = silverSprite;
            }
            else
            {
                bannerImage.sprite = goldSprite;
            }
        }
    }
}

