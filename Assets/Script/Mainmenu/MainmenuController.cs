using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TGA.Utilities;
using System.Linq;
using TGA.UI;
using UnityEditor;
using UnityEngine.EventSystems;


public class MainmenuController : MonoBehaviour
{
    public static MainmenuController instance;

    public GameObject logoContent;
    public GameObject mainmenuTimeline;
    private bool isShowingMainmenu;
    private LoadingPopup loadingPopup;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Button findroombutton;

    [SerializeField]
    private Button createRoomtitle;

    [SerializeField]
    private Button startgamebutton;

    [SerializeField]
    private Button errorbutton;

    [SerializeField]
    private Button createRoom;

    [SerializeField]
    private Button leaveRoom;

    [SerializeField]
    private Button backfindroom; 

    [SerializeField] Menu[] menus;


    [SerializeField] GameObject mainMenuWorldCanvas;

    [SerializeField] private Button NewcreateRoomtitle;

    private void Awake()
    {
        instance = this;
        SharedContext.Instance.Add(this);

        loadingPopup = SharedContext.Instance.Get<LoadingPopup>();
    }

    void Start()
    {
        playButton.onClick.AddListener(onClickPlayButton);
        exitButton.onClick.AddListener(onClickExitButton);
        errorbutton.onClick.AddListener(onClickReturntoMenu);
        findroombutton.onClick.AddListener(onClickFindRoomButton);
        createRoomtitle.onClick.AddListener(onClickCreateRoomtitle);

        NewcreateRoomtitle.onClick.AddListener(onClickCreateRoomtitle);

        startgamebutton.onClick.AddListener(onClickStartgame);
        createRoom.onClick.AddListener(onClickCreateRoom);
        leaveRoom.onClick.AddListener(onLeaveRoom);
        backfindroom.onClick.AddListener(onClickReturntoMenu);
    }

    void Update()
    {
        if(!isShowingMainmenu && Input.anyKey) 
        {
            mainmenuTimeline.SetActive(true);
            logoContent.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E))
        {

        }
    }


    private void OnDestroy()
    {
        SharedContext.Instance.Remove(this);
    }

    public void FadeBlack()
    {
        loadingPopup.FadeBlack(false);
    }

    private void onClickPlayButton()
    {
        Debug.Log("================== Play Game ===================");
    }

    private void onClickExitButton()
    {
        Debug.Log("================== Exit Game ===================");
    }

    private void onClickReturntoMenu()
    {
        OpenMenu("title");
    }

    private void onClickFindRoomButton()
    {
        OpenMenu("findroom");
    }

    public void OnPointerClick()
    {

    }

    private void onClickCreateRoomtitle()
    {
        //OpenMenu("createroom");
        Uitranform uitran = NewcreateRoomtitle.GetComponentInParent<Uitranform>();
        uitran.onUiClose();
    }

    private void onClickCreateRoom()
    {
        PhotonMainMenu.instant.CreateRoom();
    }

    private void onLeaveRoom()
    {
        PhotonMainMenu.instant.Leaveroom();
    }

    private void onClickStartgame()
    {
        PhotonMainMenu.instant.StartGame();
    }
    public void OpenMenu(string menuName)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {         
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    ((IPointerClickHandler)instance).OnPointerClick(eventData);
    //}
}
