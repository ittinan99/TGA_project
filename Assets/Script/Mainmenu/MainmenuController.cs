using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TGA.Utilities;

public class MainmenuController : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button exitButton;

    private void Awake()
    {
        SharedContext.Instance.Add(this);
    }

    void Start()
    {
        playButton.onClick.AddListener(onClickPlayButton);
        exitButton.onClick.AddListener(onClickExitButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        SharedContext.Instance.Remove(this);
    }

    private void onClickPlayButton()
    {
        Debug.Log("================== Play Game ===================");
    }

    private void onClickExitButton()
    {
        Debug.Log("================== Exit Game ===================");
    }
}
