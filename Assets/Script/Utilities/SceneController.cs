using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TGA.Utilities;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

namespace TGA.SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField]
        private GameObject loadingCanvas;

        [SerializeField]
        private Slider loadingSlider;

        [SerializeField]
        private Image FadeBlackImage;

        private float targetValue;
        private float currentValue;

        [SerializeField]
        private List<Sprite> bgList = new List<Sprite>();

        //[SerializeField]
        //private LoadingPopupBackGround LPBG;

        [SerializeField]
        private Image bgImage;

        public delegate void OnLoadingSceneFinishedCallback();
        public event OnLoadingSceneFinishedCallback OnLoadingSceneFinished;

        private bool isFadingBlack = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SharedContext.Instance.Add(this);
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadScene(sceneName));
        }

        public void ChangeBG()
        {
            var bgIndex = UnityEngine.Random.Range(0, bgList.Count - 1);
            bgImage.sprite = bgList[bgIndex];
        }

        IEnumerator LoadScene(string sceneName)
        {
            yield return null;
            //LPBG.Reset();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            //loadingCanvas.SetActive(true);
            //loadingSlider.value = 0;
            //targetValue = 0;
            //currentValue = 0;

            //var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            //if (cameraData.cameraStack.Count > 0)
            //{
            //    bool cameraUIExist = false;
            //    foreach (Camera stackCamera in cameraData.cameraStack)
            //    {
            //        if (stackCamera.gameObject.name == "UICamera")
            //        {
            //            loadingCanvas.GetComponent<Canvas>().worldCamera = stackCamera;
            //            cameraUIExist = true;
            //        }
            //    }
            //    if (cameraUIExist == false)
            //    {
            //        loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            //    }
            //}
            //else
            //{
            //    loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            //}

            //var bgIndex = UnityEngine.Random.Range(0, bgList.Count - 1);
            //bgImage.sprite = bgList[bgIndex];

            //LPBG.Fade();

            while (!asyncOperation.isDone)
            {
                targetValue = asyncOperation.progress / 0.9f;
                currentValue = Mathf.MoveTowards(currentValue, targetValue, 0.25f * Time.deltaTime);
                //loadingSlider.value = currentValue;

                if (Mathf.Approximately(currentValue, 1))
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            OnLoadingSceneFinished?.Invoke();
            //cameraData = Camera.main.GetUniversalAdditionalCameraData();
            //if (cameraData.cameraStack.Count > 0)
            //{
            //    bool cameraUIExist = false;
            //    foreach (Camera stackCamera in cameraData.cameraStack)
            //    {
            //        if (stackCamera.gameObject.name == "UICamera")
            //        {
            //            loadingCanvas.GetComponent<Canvas>().worldCamera = stackCamera;
            //            cameraUIExist = true;
            //        }
            //    }
            //    if(cameraUIExist == false)
            //    {
            //        loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            //    }
            //}
            //else
            //{
            //    loadingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            //}

            //LPBG.Reset();
            //FadeBlack();
            //yield return new WaitForSeconds(1f);
            //loadingCanvas.SetActive(false);
        }

        public void FadeBlack()
        {
            //TODO : Fade black in and out
            isFadingBlack = true;
            StartCoroutine(FadeBlackCoroutine());
            Debug.Log("Fade Black");
        }

        IEnumerator FadeBlackCoroutine()
        {
            while (FadeBlackImage.color.a < 1)
            {
                var tempColor = FadeBlackImage.color;
                tempColor.a += 0.05f;
                FadeBlackImage.color = tempColor;
                yield return new WaitForSeconds(0.025f);
            }

            yield return new WaitForSeconds(0.5f);

            while (FadeBlackImage.color.a > 0)
            {
                var tempColor = FadeBlackImage.color;
                tempColor.a -= 0.05f;
                FadeBlackImage.color = tempColor;
                yield return new WaitForSeconds(0.025f);
            }

            isFadingBlack = false;
        }
    }
}

