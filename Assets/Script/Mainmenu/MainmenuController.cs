using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TGA.Utilities;
using System.Linq;

public class MainmenuController : MonoBehaviour
{
    public static MainmenuController instance; 

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

    private void Awake()
    {
        instance = this;
        SharedContext.Instance.Add(this);
    }

    void Start()
    {
        playButton.onClick.AddListener(onClickPlayButton);
        exitButton.onClick.AddListener(onClickExitButton);
        errorbutton.onClick.AddListener(onClickReturntoMenu);
        findroombutton.onClick.AddListener(onClickFindRoomButton);
        createRoomtitle.onClick.AddListener(onClickCreateRoomtitle);
        startgamebutton.onClick.AddListener(onClickStartgame);
        createRoom.onClick.AddListener(onClickCreateRoom);
        leaveRoom.onClick.AddListener(onLeaveRoom);
        backfindroom.onClick.AddListener(onClickReturntoMenu);
    }

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

    private void onClickReturntoMenu()
    {
        OpenMenu("title");
    }

    private void onClickFindRoomButton()
    {
        OpenMenu("findroom");
    }

    private void onClickCreateRoomtitle()
    {
        OpenMenu("createroom");
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
}
