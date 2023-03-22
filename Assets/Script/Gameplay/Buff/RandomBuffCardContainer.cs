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
        [SerializeField] private TextMeshProUGUI SelectedText;

        private PhotonView pv;
        private RectTransform rt;
        private RandomBuffController randomBuffController;

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            rt = GetComponent<RectTransform>();
            randomBuffController = SharedContext.Instance.Get<RandomBuffController>();
        }

        private void Update()
        {
            SelectedText.text = Selected.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            rt.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
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
            }
        }

        public void Populate(RandomBuffCard goodBuffCard, RandomBuffCard badBuffCard)
        {
            GoodBuffCard = goodBuffCard;
            BadBuffCard = badBuffCard;

            goodBuffName.text = goodBuffCard.name;
            goodBuffDescription.text = goodBuffCard.Description;

            badBuffName.text = badBuffCard.name;
            badBuffDescription.text = badBuffCard.Description;
        }
    }
}

