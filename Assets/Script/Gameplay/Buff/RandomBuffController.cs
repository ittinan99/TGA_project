using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TGA.Utilities;
using Photon.Pun;
using System.Linq;

namespace TGA.Gameplay
{
    public class RandomBuffController : MonoBehaviour
    {
        PhotonView PV;

        [Header("Buffs and Debuff")]
        [SerializeField]
        private List<RandomBuffCard> GoodBuffList;

        [SerializeField]
        private List<RandomBuffCard> BadBuffList;

        private List<string> randGoodBuffList = new List<string>();
        private List<string> randBadBuffList = new List<string>();

        [Space(10)]

        [Header("Container")]
        [SerializeField]
        private GameObject randomBuffCanvas;
        [SerializeField]
        private List<RandomBuffCardContainer> buffContainerList;

        public RandomBuffCardContainer selectedContainer;

        private EnemySpawnManager enemySpawnManager;
        [Space(10)]

        [Header("Timer")]
        [SerializeField]
        private Slider pickCardTimer;
        public float pickCardTime;
        private Coroutine pickCardCoroutine;

        private void Awake()
        {
            PV = GetComponent<PhotonView>();

            SharedContext.Instance.Add(this);

            //if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }
        }

        void Update()
        {

        }

        public void Initialize(EnemySpawnManager enemySpawnManager)
        {
            this.enemySpawnManager = enemySpawnManager;

            foreach(var buffCard in GoodBuffList)
            {
                for(int i= 0; i < buffCard.Rarity; i++)
                {
                    randGoodBuffList.Add(buffCard.Id);
                }
            }

            foreach (var buffCard in BadBuffList)
            {
                for (int i = 0; i < buffCard.Rarity; i++)
                {
                    randBadBuffList.Add(buffCard.Id);
                }
            }

            //    Shuffle       ======================

            shuffleCard();
            randomBuffCanvas.SetActive(false);
        }

        private void shuffleCard()
        {
            for (int i = 0; i < randGoodBuffList.Count; i++)
            {
                string temp = randGoodBuffList[i];
                int randomIndex = Random.Range(i, randGoodBuffList.Count);
                randGoodBuffList[i] = randGoodBuffList[randomIndex];
                randGoodBuffList[randomIndex] = temp;
            }

            for (int i = 0; i < randBadBuffList.Count; i++)
            {
                string temp = randBadBuffList[i];
                int randomIndex = Random.Range(i, randBadBuffList.Count);
                randBadBuffList[i] = randBadBuffList[randomIndex];
                randBadBuffList[randomIndex] = temp;
            }
        }

        private RandomBuffCard GetRandomGoodBuffCard()
        {
            int rand = Random.Range(0, randGoodBuffList.Count);

            var goodBuffId = randGoodBuffList[rand];

            var randomGoodBuffCard = GoodBuffList.Find(x => x.Id == goodBuffId);

            randGoodBuffList.RemoveAll(x => x == goodBuffId);

            return randomGoodBuffCard;
        }

        private RandomBuffCard GetRandomBadBuffCard()
        {
            int rand = Random.Range(0, randBadBuffList.Count);

            var badBuffId = randBadBuffList[rand];

            var randomBadBuffCard = BadBuffList.Find(x => x.Id == badBuffId);

            randBadBuffList.RemoveAll(x => x == badBuffId);

            return randomBadBuffCard;
        }

        public void PopulateRandomBuffCard()
        {
            enemySpawnManager.IsPreparing = true;
            Cursor.lockState = CursorLockMode.None;

            pickCardCoroutine = StartCoroutine(pickRandomBuffCard());

            var goodBuffs = new List<string>();
            var badBuffs = new List<string>();

            for(int i = 0; i < buffContainerList.Count; i++)
            {
                goodBuffs.Add(GetRandomGoodBuffCard().Id);
                badBuffs.Add(GetRandomBadBuffCard().Id);
            }

            string randomGoodBuffIds = string.Join(',', goodBuffs);
            string randomBadBuffIds = string.Join(',', badBuffs);

            PV.RPC("RPC_PopulateRandomBuffCard", RpcTarget.All, randomGoodBuffIds, randomBadBuffIds);
        }

        [PunRPC]
        private void RPC_PopulateRandomBuffCard(string randomGoodBuffIds, string randomBadBuffIds)
        {
            randomBuffCanvas.SetActive(true);

            List<string> goodBuffIdList = randomGoodBuffIds.Split(',').ToList();
            List<string> badBuffIdList = randomBadBuffIds.Split(',').ToList();

            for (int i = 0; i < buffContainerList.Count; i++)
            {
                RandomBuffCard goodBuffCard = GoodBuffList.Find(x => x.Id == goodBuffIdList[i]);
                RandomBuffCard badBuffCard = BadBuffList.Find(x => x.Id == badBuffIdList[i]);

                buffContainerList[i].Populate(goodBuffCard, badBuffCard);
            }
        }

        IEnumerator pickRandomBuffCard()
        {
            var timer = pickCardTime;
            pickCardTimer.maxValue = timer;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                pickCardTimer.value = timer;
                yield return null;
            }

            pickCardTimer.value = 0;
            FinishPickRandomCard();
        }

        private void FinishPickRandomCard()
        {
            enemySpawnManager.IsPreparing = false;
            Cursor.lockState = CursorLockMode.Locked;

            float playerCount = GameObject.FindGameObjectsWithTag("Player").Length;

            var selectedCards = buffContainerList.FindAll(x => x.Selected >= playerCount / 2f);

            var selectedCard = buffContainerList[0];

            if (selectedCards.Count > 1)
            {
                int rand = Random.Range(0, selectedCards.Count);

                selectedCard = selectedCards[rand];

                Debug.Log($"================== {selectedCard.GoodBuffCard.name} & {selectedCard.BadBuffCard.name} Selected ==================");
            }
            else
            {
                if(selectedCards.Count > 0)
                {
                    selectedCard = selectedCards[0];

                    Debug.Log($"================== {selectedCard.GoodBuffCard.name} & {selectedCard.BadBuffCard.name} Selected ==================");
                }
                else
                {
                    int rand = Random.Range(0, buffContainerList.Count);

                    selectedCard = buffContainerList[rand];

                    Debug.Log($"================== {selectedCard.GoodBuffCard.name} & {selectedCard.BadBuffCard.name} Selected ==================");
                }
            }

            // Add non selected card back to List

            foreach (var buffCard in selectedCards)
            {
                if(buffCard == selectedCard) { continue; }

                for (int i = 0; i < buffCard.GoodBuffCard.Rarity; i++)
                {
                    randGoodBuffList.Add(buffCard.GoodBuffCard.Id);
                }

                for (int i = 0; i < buffCard.BadBuffCard.Rarity; i++)
                {
                    randBadBuffList.Add(buffCard.BadBuffCard.Id);
                }
            }

            shuffleCard();
            ApplyEffectOfCard(selectedCard);
            randomBuffCanvas.SetActive(false);
        }

        private void ApplyEffectOfCard(RandomBuffCardContainer cardContainer)
        {
            Debug.Log($"================== Apply Effect of Selected Card ==================");
        }

        private void OnDestroy()
        {
            SharedContext.Instance.Remove(this);
        }
    }
}

